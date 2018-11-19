using System;
using System.Xml;
using System.Xml.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Data;
using System.Threading;
using System.Data.OleDb;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using Office = Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

namespace RightSignature
{
  
  public class Start  {


      [STAThread]
    static void Main(string[] args)
    {

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Form1());

    }


  }
}