// ‘Affirmation of Authorship:
// ‘Name: Mohammad Baig 
// ‘Date: 10/13/2024
// ‘I affirm that this program was created by me. It is solely my work and ‘does not include any work
// done by anyone else.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment3
{
    public partial class Form1 : Form
    {
        private List<string> fileList = new List<string>();
        private List<string> filePaths = new List<string>();
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            string folderPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\TaskFiles";

            try
            {
                // Get all file paths in the directory
                string[] files = System.IO.Directory.GetFiles(folderPath);

                // Clear the existing items in the ListBox before adding new ones
                listBox1.Items.Clear();
                filePaths.Clear(); // Clear previous paths

                // Iterate through the files to separate names and paths
                foreach (var file in files)
                {
                    filePaths.Add(file); // Store the full path

                    // Add the file name without extension to the ListBox
                    listBox1.Items.Add(System.IO.Path.GetFileNameWithoutExtension(file));
                }
            }
            catch (Exception ex)
            {
                // Handle any errors (e.g., directory doesn't exist)
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Create an instance of the OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set the filter options for file types (optional)
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            // Set initial directory (optional)
            openFileDialog.InitialDirectory = @"C:\";

            // Show the dialog and check if the user selected a file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get the selected file path
                string filePath = openFileDialog.FileName;

                // Extract just the file name without the extension and set it to textBox1
                textBox1.Text = System.IO.Path.GetFileNameWithoutExtension(filePath);

                Form2 form2 = new Form2(filePath);
                form2.ShowDialog();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //create file btn

            if (textBox1.Text.Length == 0) return;

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Define the folder path for "TaskFiles"
            string folderPath = $@"{desktopPath}\TaskFiles";

            // Create the "TaskFiles" folder if it doesn't exist
            System.IO.Directory.CreateDirectory(folderPath);

            // Define the file path within the "TaskFiles" folder
            string filePath = $@"{folderPath}\{textBox1.Text}.txt";

            try
            {
                if (!System.IO.File.Exists(filePath))
                {
                    // Create an empty file in the "TaskFiles" folder
                    using (System.IO.FileStream fs = System.IO.File.Create(filePath))
                    {
                        // FileStream must be closed after creation
                    }

                    // Notify the user
                    MessageBox.Show($"File '{filePath}' has been created successfully in 'TaskFiles' folder.", "File Created", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"File '{filePath}' already exists in 'TaskFiles' folder.", "File Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions (e.g., access denied, path invalid)
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ///logic to re render the list
            ///

            try
            {
                // Get all file paths in the directory
                string[] files = System.IO.Directory.GetFiles(folderPath);

                // Clear the existing items in the ListBox before adding new ones
                listBox1.Items.Clear();
                filePaths.Clear(); // Clear previous paths

                // Iterate through the files to separate names and paths
                foreach (var file in files)
                {
                    filePaths.Add(file); // Store the full path

                    // Add the file name without extension to the ListBox
                    listBox1.Items.Add(System.IO.Path.GetFileNameWithoutExtension(file));
                }
            }
            catch (Exception ex)
            {
                // Handle any errors (e.g., directory doesn't exist)
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
