using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Threading;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace RightSignature
{
    public partial class Form1 : Form
    {
        private static List<Structs.MergeField> mergefields = null;
        private static Dictionary<string,string> templatemergefields = null;

        private static DataTable sheetData;

        private RightSignatureRequest rsRequest;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            rsRequest = new RightSignatureRequest();

            templatemergefields = new Dictionary<string, string>();
 
            //Get list of Templates from Right Signature
            try
            {  
                dynamic templates = rsRequest.GetReusableTemplates();
                foreach (dynamic template in templates)
                {
                    string result = template.ToString();
                    TemplateComboBox.Items.Add((string)template.id + " ~ " + (string)template.name);
                }

            }
            catch (Exception my_e)
            {
                MessageBox.Show(my_e.ToString());
                Application.Exit();
            }
            
        }

        private void openFileBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Choose Merge File";
            theDialog.Filter = "Excel Files|*.xls;*.xlsx";
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
               textBox2.Text = theDialog.FileName.ToString();
            }

        }

        private void sendContractsBtn_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";

            if(textBox2.Text == "")
            {
                MessageBox.Show("Please Select a Merge File");
                return;
            }

            if (TemplateComboBox.SelectedItem == null || TemplateComboBox.SelectedItem.ToString() == "")
            {
                MessageBox.Show("Please select a tempate to use");
                return;
            }

            List<string> colname = new List<string>();
            try
            {
                sheetData = openExcel(textBox2.Text);              
                foreach (DataColumn c in sheetData.Columns)
                {
                    colname.Add(c.ColumnName);
                    textBox1.Text += "Excel Column Name:" + c.ColumnName + "\r\n";
                }
            }
            catch (Exception my_e)
            {
                MessageBox.Show("Unable to parse Excel File " + textBox2.Text + " : " + my_e.ToString());
                return;
            }

            string myguid = TemplateComboBox.SelectedItem.ToString();
            myguid = myguid.Substring(0, myguid.IndexOf("~") - 1);

            try
            {
                textBox1.Text += "Parsing Merge Fields\r\n";
                getTemplateMergeFields(myguid);
                textBox1.Text += "Finished Parsing Merge Fields\r\n";

            }
            catch (Exception my_e)
            {
                MessageBox.Show("Unable to get template merge fields. Do you have the right template? - " + my_e.ToString());
                return;
            }

            foreach (string s in templatemergefields.Keys)
            {
                textBox1.Text += "Template Column Name:" + s + "\r\n";
            }

            bool missing_columns = false;
            foreach (string s in templatemergefields.Keys)
            {
                if (colname.Contains(s))
                {
                    //Excel Sheet has the right column name
                }
                else
                {
                    textBox1.Text +=  "Missing Column: " + s + "\r\n";
                    missing_columns = true;
                }
            }

            if(missing_columns)
            {
                MessageBox.Show("There are required merge fields missing from the Excel file. Check the textbox for further information.");
                return;
            }


            String mysubject = emailSubjectBox.Text;
            double est_time = sheetData.Rows.Count * 1.8/60;
            DialogResult result1 = MessageBox.Show("Send Contracts? - This process cannot be stopped after this point and may take " + est_time.ToString() + " hours or more. The subject line of the email will be \r\n" + mysubject, "Send Contracts?",MessageBoxButtons.YesNo);

            if(result1 == DialogResult.No)
            {
                return;
            }


            int i = 0;
            foreach (DataRow dr in sheetData.Rows)
            {
                textBox1.AppendText("\r\n !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! \r\n");
                textBox1.Text += "\r\n Sending contract #" + i + "\r\n";
                textBox1.AppendText("\r\n !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! \r\n");

                JObject d = new JObject();
                d.Add("name", emailSubjectBox.Text);
                d.Add("message", "Please sign this document ");
                d.Add("expires_in", "30");

                JArray roles = new JArray();
                d.Add(new JProperty("roles", roles));

                JObject p1 = new JObject();
                p1.Add("name", "parent_1");
                //p1.Add("name", "intl_father_name");

                JObject p2 = new JObject();
                p2.Add("name", "parent_2");
                //p2.Add("name", "intl_mother_name");

                JObject sgs_signer = new JObject();
                sgs_signer.Add("name", "Sender");
                sgs_signer.Add("signer_email", "tami.peplinski@sgs.org");
                sgs_signer.Add("signer_name", "Jamie Tender");


                roles.Add(p1);
                roles.Add(p2);
                roles.Add(sgs_signer);

                JArray mf = new JArray();
                d.Add(new JProperty("merge_field_values", mf));

                if (dr.Table.Columns.Contains("intl_father_name"))
                {
                    if (dr["intl_father_name"].ToString() != "" && dr["intl_father_email"].ToString() != "")
                    {
                        string parent = dr["intl_father_name"].ToString();
                        string email = dr["intl_father_email"].ToString().Trim();

                        p1.Add("signer_email", email);
                        p1.Add("signer_name", parent);
                    }
                    else
                    {
                        //Error Missing Parent 1
                        textBox1.Text += "Error - Missing Parent 1 - Skipping Record\r\n";

                        //Skip this record
                        continue;
                    }
                }
                else
                {
                    if (dr["U_Students_Extension.parent1_last"].ToString() != "" && dr["U_Students_Extension.parent1_email1"].ToString() != "")
                    {
                        string parent = dr["U_Students_Extension.parent1_first"].ToString() + " " + dr["U_Students_Extension.parent1_last"].ToString();
                        string email = dr["U_Students_Extension.parent1_email1"].ToString().Trim();

                        p1.Add("signer_email", email);
                        p1.Add("signer_name", parent);
                    }
                    else
                    {
                        //Error Missing Parent 1
                        textBox1.Text += "Error - Missing Parent 1 - Skipping Record\r\n";

                        //Skip this record
                        continue;
                    }
                }

                if (dr.Table.Columns.Contains("intl_mother_name"))
                {
                    if (dr["intl_mother_name"].ToString() != "" && dr["intl_mother_email"].ToString() != "")
                    {
                        string parent = dr["intl_mother_name"].ToString();
                        string email = dr["intl_mother_email"].ToString().Trim();

                        p2.Add("signer_email", email);
                        p2.Add("signer_name", parent);
                    }
                    else
                    {
                        //Warning - Missing Parent 2
                        p2.Add("signer_omitted", true);
                        textBox1.Text += "Warning - Missing Parent 2\r\n";
                    }
                }
                else
                {
                    if (dr["U_Students_Extension.parent2_last"].ToString() != "" && dr["U_Students_Extension.parent2_email1"].ToString() != "")
                    {
                        string parent = dr["U_Students_Extension.parent2_first"].ToString() + " " + dr["U_Students_Extension.parent2_last"].ToString();
                        string email = dr["U_Students_Extension.parent2_email1"].ToString().Trim();

                        p2.Add("signer_email", email);
                        p2.Add("signer_name", parent);
                    }
                    else
                    {
                        //Warning - Missing Parent 2
                        p2.Add("signer_omitted", true);
                        textBox1.Text += "Warning - Missing Parent 2\r\n";
                    }
                }
                textBox1.AppendText("Reading Merge Field Values\r\n");

                mergefields = new List<Structs.MergeField>();
                foreach (KeyValuePair<string,string> entry in templatemergefields)
                {
                    JObject mfv = new JObject();
                    mf.Add(mfv);
                    mfv.Add("id", entry.Value);
                    string value = dr[entry.Key].ToString();
                    if(value == "")
                    {
                        mfv.Add("value", "_");
                    }
                    else
                    {
                        
                        mfv.Add("value", value);
                    }
                    
                    
                }


                
                textBox1.AppendText("Sending Contract\r\n");
                try
                {
                    string json = d.ToString();
                    sendTemplate(myguid, json); 
                }
                catch(Exception my_e)
                {
                    textBox1.AppendText("Error sending contract #" + i + "\r\n");
                    textBox1.AppendText(my_e.ToString() + "\r\n");
                    textBox1.AppendText("Trying to continue\r\n");

                    Thread.Sleep(500);
                    i++;
                    continue;

                }

                textBox1.AppendText("\r\n Thread Sleeping for 29,000 ms \r\n");
                for (int q = 0; q < 80;q++)
                { 
                    Thread.Sleep(250);
                    Application.DoEvents();
                }
                i++;

            }

            textBox1.AppendText("All Done! \r\n");
        }

        private static DataTable openExcel(string filename)
        {
            object misValue = System.Reflection.Missing.Value;

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(filename);
            Excel._Worksheet xlWorksheet1 = xlWorkbook.Sheets[1];
            Excel.Range xlRange1 = xlWorksheet1.UsedRange;

            DataTable excelTb1 = new DataTable();
            excelTb1.CaseSensitive = true;

            for (int j = 1; j <= xlRange1.Columns.Count; j++) // Header Names
            {
                excelTb1.Columns.Add(xlRange1.Cells[1, j].Value2.ToString());
            }

            DataRow dataRow = null;

            for (int row = 2; row < xlRange1.Rows.Count + 1; row++)
            {
                dataRow = excelTb1.NewRow();

                for (int col = 1; col <= xlRange1.Columns.Count; col++)
                {
                    dataRow[col - 1] = (xlRange1.Cells[row, col] as Excel.Range).Value2;
                }
                excelTb1.Rows.Add(dataRow);
            }

            xlWorkbook.Close(true, misValue, misValue);
            xlApp.Quit();

            return excelTb1;
        }

        private void getTemplateMergeFields(string myguid)
        {
            dynamic template = rsRequest.GetTemplateMergeFields(myguid);
        
            foreach (dynamic t in template["merge_field_components"])
            {
                if (templatemergefields.ContainsKey((string)t.name))
                {
                    //Duplicate merge field in template
                    textBox1.AppendText("\r\n !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! \r\n");
                    textBox1.AppendText("\r\n Duplicate Merge Field name in document: " + (string)t.name +  "\r\n");
                    textBox1.AppendText("\r\n !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! \r\n");
                }
                else
                {
                    templatemergefields.Add((string)t.name, (string)t.id);
                }
            }

        }
 
        private void sendTemplate(string id, string json)
        {
            try
            {
                textBox1.AppendText(rsRequest.SendTempate(id, json));
            }
            catch(Exception my_e)
            {
                textBox1.AppendText("/r/n !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! /r/n");
                textBox1.AppendText("Failed to send contract with ID:" + id + " and JSON: " + json + "/r/n Error:" + my_e.ToString() + "\r\n");
                textBox1.AppendText("/r/n !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! /r/n");
                return;
            }
        }
    }
}
