using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace something
{
    public partial class MainWindow : Window
    {
        private string Filename;
        private static string DirPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/MyNoteProgram";
        List<string> NotesList = new List<string>();
    
        public MainWindow()
        {
            InitializeComponent();
            DirectoryCreate();
            Avoidexc();
            StartInit();
        }
        private void DirectoryCreate()
        {
            //Creating directory where all notes exist
            try
            {
                if (!Directory.Exists(DirPath))
                {
                   DirectoryInfo dI = Directory.CreateDirectory(DirPath);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Process of creating folder with notes failed. " + e.ToString());
            }
        }
        private void Avoidexc()
        {
            //In case if there's no notes to address
            FileInfo fileInfo = new FileInfo(DirPath + "/" + "DefaultNote.txt");
            if (!fileInfo.Exists)
                fileInfo.Create().Close();
        }
        private void StartInit()
        {   
            //Starting Initialization...
            var directoryInfo = new DirectoryInfo(DirPath);
            var fileInfos = directoryInfo.GetFiles();          

            for (int i = 0; i < fileInfos.Length; i++)
            {
                string Temp = fileInfos[i].Name;
                string TempOk = Temp.Substring(0, Temp.Length-4);

                NotesList.Add(TempOk);               
            }

            for (int i = 0; i < NotesList.Count; i++)
            { 
                NotesPanel.Items.Add(NotesList[i]);
            }
    
            NotesPanel.SelectedIndex = 0;
        }
        private void WriteFileOnSave()
        {
            //Method to Write text when you save file
            Filename = NoteNameEditor.Text;
            using (StreamWriter sw = new StreamWriter(DirPath + "/" + Filename + ".txt", false))
            {
                sw.Write(NoteEditor.Text);
            }
        }
        private void FileCreator()
        {
            //Create file on save if there's no notes with the same name
            Filename = NoteNameEditor.Text;
            FileInfo fileInf = new FileInfo(DirPath + "/" + Filename + ".txt");
            if (!fileInf.Exists)
            {
                fileInf.Create().Close();  
                NotesPanel.Items.Add(Filename);
            }
    
        }
        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            //Save Button
            FileCreator();
            WriteFileOnSave();
        }
        private void ListBoxMainSelectChanged(object sender, RoutedEventArgs e)
        {
            //Event that triggers changing on click to different note
            NoteNameEditor.Text = NotesPanel.SelectedItem.ToString();
            Filename = NoteNameEditor.Text;

            if (File.Exists(DirPath + "/" + Filename + ".txt"))            
                reaDer();        

            if (NotesPanel.Items.Count <= 0)
                return;

            if(NotesPanel.SelectedIndex == -1)
            {
                NotesPanel.SelectedIndex = 0;              
            }      
        }
        private void reaDer()
        {
            //Method that reads file
            using (StreamReader sr = new StreamReader(DirPath + "/" + Filename + ".txt"))
            {
                NoteEditor.Text = sr.ReadToEnd();
            }
        }
        private void DeleteNoteButton_Click(object sender, RoutedEventArgs e)
        {
            //Delete Button
            FileInfo fileinf = new FileInfo(DirPath + "/" + NotesPanel.SelectedItem + ".txt");
            fileinf.Delete();

            //Delete Extension of file
            string fileNameWithExtension = fileinf.Name;
            string fileNameNoExtension = fileNameWithExtension.Substring(0, fileNameWithExtension.Length - 4);

            if (NotesPanel.SelectedIndex != 0)
            {
                NotesPanel.SelectedIndex--;
            }
            else
            {
                NotesPanel.SelectedIndex++;
            }

            NotesPanel.Items.Remove(fileNameNoExtension);
        }
        private void prevButtonClick(object sender, RoutedEventArgs e)
        {
            //previous note selector
            if(NotesPanel.SelectedIndex == 0)
            {
                NotesPanel.SelectedIndex = NotesPanel.Items.Count - 1;
            }
            else
            {
                NotesPanel.SelectedIndex--;
            }
            
        }
        private void nextButtonClick(object sender, RoutedEventArgs e)
        {
            //next note selector
            if(NotesPanel.SelectedIndex == NotesPanel.Items.Count - 1)
            {
                NotesPanel.SelectedIndex = 0;
            }
            else
            {
                NotesPanel.SelectedIndex++;
            }        
        }
    }
}
