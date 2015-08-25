#region Copyright
//
// Copyright (C) 2015 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//
// Written by M.Harada
// 
#endregion // Copyright

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics; // for debugging. 
using System.IO; 
// Added for RestSharp. 
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers; 

namespace HelloFieldWorld
{
    public partial class Form1 : Form
    {
        // Save ticket. 
        private static string m_ticket = "";
        private static string m_project_id = ""; // current project id 
        private static int m_proj_index = 0;     // index in the list of projects 
        private static string m_equipment_id = "";
        private static string m_fileName1 = "";
        //private static string m_fileName2 = ""; 
        // 
        private static List<Project> m_project_list = null;
        //private static List<Equipment> m_equipment_list = null; 

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            // Get the user name and password from the user. 
            string userName = textBoxUserName.Text;
            string password = textBoxPassword.Text;

            textBoxRequest.Text = "Request comes here";
            textBoxResponse.Text = "Response comes here. This may take secones. Please wait...";
            this.Update();

            // Here is the main part that we call Glue login 
            m_ticket = Field.Login(userName, password);

            // For our learning, 
            // show the request and response in the form. 
            ShowRequestResponse(); 
        }

        // show the request and response in the form. 
        private void ShowRequestResponse()
        {
            IRestResponse response = Field.m_lastResponse;
            textBoxRequest.Text = response.ResponseUri.AbsoluteUri;
            textBoxResponse.Text = 
                "Status Code: " + response.StatusCode.ToString() 
                + Environment.NewLine + response.Content;
        }

        private void buttonProjects_Click(object sender, EventArgs e)
        {
            textBoxRequest.Text = "Request comes here";
            textBoxResponse.Text = "Response comes here. This may take secones. Please wait...";
            this.Update();

            //List<Project> proj_list = Field.ProjectList(m_ticket);
            //List<Project> proj_list = Field.ProjectNames(m_ticket);
            m_project_list = Field.ProjectNames(m_ticket);

            ShowRequestResponse(); 
 
            // Make a list of project names from proj_list; 

            comboBoxProjects.Items.Clear(); 
            foreach (Project item in m_project_list) 
            {
                comboBoxProjects.Items.Add(item.name); 
            }
            comboBoxProjects.SelectedIndex = 0;

            // Display the # of projects. 
            labelProject.Text = "Project(" + m_project_list.Count.ToString() + ")"; 

        }

        private void comboBoxProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            int project_index = comboBoxProjects.SelectedIndex;
            m_project_id = m_project_list[project_index].id;
        }

        private void buttonFile1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxFile1.Text = fileDialog.FileName;  // full path 

                m_fileName1 = fileDialog.SafeFileName; // file name only  
                //m_path = fileDialog.FileName; 

                Debug.WriteLine("File name1 = " + fileDialog.SafeFileName);
                Debug.WriteLine("File full path1 = " + fileDialog.FileName);

            }
        }

        private void buttonPublish_Click(object sender, EventArgs e)
        {
            textBoxRequest.Text = "Request comes here";
            textBoxResponse.Text = "Response comes here. This may take secones. Please wait...";
            this.Update();

            string FiledataPath = textBoxFile1.Text;
            string directory = ""; // "aaa";
            string filename = m_fileName1;
            string document_id = "";
            string tags = "MyTag,YourTag";
            string caption = ""; 

            Field.LibraryPublish(m_ticket, m_project_id, directory, 
                filename, FiledataPath, document_id, tags, caption); 

            // For our learning, 
            // show the request and response in the form. 
            ShowRequestResponse(); 

        }
        
    }
}
