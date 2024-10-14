using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment3
{
    public partial class Form2 : Form
    {
        private string filePath;
        private List<TaskInfo> tasks;
        private List<string> lines;

        private string selectedColor;
        public Form2(string filePath)
        {
            InitializeComponent();
            this.filePath = filePath;

            button4.Enabled = false;
            button3.Enabled = false;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                // Read all lines from the file
                lines = System.IO.File.ReadAllLines(filePath).ToList();

                // Initialize the tasks list
                tasks = new List<TaskInfo>();

                // Clear the ListBox before populating
                listBox1.Items.Clear();

                // Process each line to create TaskInfo objects
                foreach (var line in lines)
                {
                    // Trim the line to remove any leading or trailing whitespace
                    var trimmedLine = line.Trim();

                    // Skip empty lines
                    if (string.IsNullOrWhiteSpace(trimmedLine))
                        continue;

                    // Split the line by comma
                    var parts = trimmedLine.Split(',');

                    // Ensure that the line has the expected number of parts
                    if (parts.Length == 5)
                    {
                        try
                        {
                            // Add the original line to the ListBox for display
                            listBox1.Items.Add(trimmedLine);

                            string name = parts[0]; // Task Name

                            // Attempt to parse the due date with a specific format
                            DateTime dueDateTime;
                            bool isDateParsed = DateTime.TryParseExact(parts[1] + " " + parts[2], "M/d/yyyy HH:mm",
                                System.Globalization.CultureInfo.InvariantCulture,
                                System.Globalization.DateTimeStyles.None,
                                out dueDateTime);

                            if (!isDateParsed)
                            {
                                throw new FormatException("Due date is not in the correct format.");
                            }

                            // Parse the completed status
                            bool completed = bool.Parse(parts[3]); // Ensure this is either "true" or "false"

                            string color = parts[4]; // Color

                            // Create a new TaskInfo object and add it to the tasks list
                            tasks.Add(new TaskInfo
                            {
                                Name = name,
                                DueDateTime = dueDateTime,
                                Complete = completed,
                                Category = color
                            });
                        }
                        catch (FormatException ex)
                        {
                            // Handle specific format errors for dueDateTime or completed status
                            MessageBox.Show($"Error parsing line: '{trimmedLine}'. {ex.Message}", "Format Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        catch (Exception ex)
                        {
                            // Handle any other errors
                            MessageBox.Show($"Error processing line: '{trimmedLine}'. {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Line format error: '{trimmedLine}' does not contain required data.", "Format Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that may occur (e.g., file not found)
                MessageBox.Show($"Error reading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {
            //color 

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button3.Enabled = listBox1.SelectedIndex >= 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    Color color = colorDialog.Color;
                    selectedColor = color.Name; // Display color name
                    label9.ForeColor = color;
                    label9.Text = "Color" + "["+selectedColor+"]";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Add button functionality

            // Check if the name or color is empty
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Please enter a task name.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(selectedColor)) // Ensure selectedColor is defined and populated elsewhere
            {
                MessageBox.Show("Please select a color.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Create a new TaskInfo object
            TaskInfo newTask = new TaskInfo
            {
                Name = textBox1.Text, // Use the name from textBox1
                DueDateTime = DateTime.Now, // Set to current date and time
                Complete = checkBox1.Checked,
                Category = selectedColor // Use the selected color
            };

            // Format the DueDateTime to include both date and time without a space
            string formattedDateTime = newTask.DueDateTime.ToString("MM/dd/yyyy,HH:mm"); // No space between date and time

            // Prepare the line format for display
            string lineToAdd = $"{newTask.Name},{formattedDateTime},{newTask.Complete},{newTask.Category}";

            // Add to the tasks list
            tasks.Add(newTask);

            // Add the line to the listbox
            listBox1.Items.Add(lineToAdd);

            // Add the line to the lines list (to keep track of all lines)
            lines.Add(lineToAdd);

            // Clear the text box and reset the selected color
            textBox1.Clear();
            selectedColor = string.Empty; // Clear the selected color (ensure you reset accordingly)
            label9.Text = string.Empty;
            label9.ForeColor = SystemColors.ControlText;

            button4.Enabled = true; // Enable the Save button after adding a task
        }



        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Check if an item is selected in the listBox
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a task to remove.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the selected index
            int selectedIndex = listBox1.SelectedIndex;

            // Remove the item from the tasks list and lines list
            if (selectedIndex >= 0 && selectedIndex < tasks.Count)
            {
                // Remove the corresponding TaskInfo object from the tasks list
                tasks.RemoveAt(selectedIndex);

                // Remove the corresponding line from the lines list
                lines.RemoveAt(selectedIndex);

                // Remove the selected item from the listBox
                listBox1.Items.RemoveAt(selectedIndex);
            }

            // Enable the Save button as changes have been made
            button4.Enabled = true;
        }


        private void button4_Click(object sender, EventArgs e)
        {
            //save btn

            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, false)) // 'false' to overwrite the file
                {
                    foreach (var line in lines)
                    {
                        writer.WriteLine(line);
                    }
                }

                MessageBox.Show("Tasks saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Handle any errors that may occur during file writing
                MessageBox.Show($"Error saving tasks to file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Optionally disable the Save button after saving
            button4.Enabled = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Prompt the user with a Yes/No dialog
            DialogResult result = MessageBox.Show(
                "Pending changes, sure you want to cancel?",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            // Check the user's choice
            if (result == DialogResult.Yes)
            {
                // User chose YES: Close the form without saving
                this.Close(); // or this.Hide(); if you want to hide instead of close
            }
            // If the user chose NO, do nothing (the form will remain open)
        }
    }
}
