using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;
using Task2.Classes;

namespace Task2.BL
{
    public class MainViewModel: INotifyPropertyChanged
    {
        public ObservableCollection<Polygon> Hexagones { get; set; }
        private Polygon CurrentHexagone { get; set; }
        private uint CountHexEdges { get; set; }
        private Color currentColor;
        public Color CurrentColor
        {
            get
            {
                return currentColor;
            }
            set
            {
                currentColor = value;
                OnPropertyChanged("CurrentColor");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainViewModel()
        {
            Hexagones = new ObservableCollection<Polygon>();
            CountHexEdges = 0;
            CurrentColor = Colors.Red;
            CurrentHexagone = new Polygon();
            ClearWindow_Command = new RelayCommand(ClearWindow);
            OpenFile_Command = new RelayCommand(OpenFile);
            SaveFile_Command = new RelayCommand(SaveFile);
            CloseWindow_Command = new RelayCommand(CloseWindow);
            DrawClick_Command = new RelayCommand(DrawClick);
            ApplyColor_Command = new RelayCommand(ApplyColor);
            SelectHexagone_Command = new RelayCommand(SelectHexagone);
            Drag_Command = new RelayCommand(Drag);
        }

        //Painting
        public ICommand DrawClick_Command { get; private set; }
        public ICommand ApplyColor_Command { get; set; }

        //File Menu
        public ICommand ClearWindow_Command { get; private set; }
        public ICommand OpenFile_Command { get; private set; }
        public ICommand SaveFile_Command { get; private set; }
        public ICommand CloseWindow_Command { get; private set; }

        //Selecting and draging hexogones
        public ICommand SelectHexagone_Command { get; private set; }
        public ICommand Drag_Command { get; private set; }
        private bool AllowDragging { get; set; }
        private Point MousePosition { get; set; }
        private Polygon SelectedHexagone { get; set; }

        
        private void DrawClick(object obj)
        {
            Point mousePoint = Mouse.GetPosition((IInputElement)obj);
            CurrentHexagone.Stroke = Brushes.Black;
            CurrentHexagone.Points.Add(mousePoint);
            if (++CountHexEdges == 6)
            {
                ColorsWindow colorWin = new ColorsWindow(this);
                if (colorWin.ShowDialog() == true)
                {
                    CurrentHexagone.Fill = new SolidColorBrush(CurrentColor);
                }
                CurrentHexagone.Name = String.Format("Hexagone_{0}", Hexagones.Count + 1);
                Hexagones.Add(CurrentHexagone);
                CurrentHexagone = new Polygon();
                OnPropertyChanged("Hexagones");
                CountHexEdges = 0;
            }
        }

        private void ApplyColor(object obj)
        {
            ColorsWindow colorsWindow = (ColorsWindow)obj;
            colorsWindow.DialogResult = true;
            colorsWindow.Close();
        }

        //File Menu
        private void ClearWindow(object obj)
        {
            Hexagones.Clear();
            OnPropertyChanged("Hexagones");
        }

        private void OpenFile(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".xml";
            openFileDialog.Filter = "XML documents (.xml)|*.xml";
            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                List<Hexagone> hexagones = new List<Hexagone>();
                XmlSerializer serializer = new XmlSerializer(typeof(List<Hexagone>));
                using (XmlReader reader = XmlReader.Create(fileName))
                {
                    hexagones = (List<Hexagone>)serializer.Deserialize(reader);
                }
                Hexagones.Clear();
                for (int i = 0; i < hexagones.Count; ++i)
                {
                    Hexagones.Add(new Polygon() { Name = String.Format("Hexagone_{0}", i + 1), Stroke = Brushes.Black, Points = hexagones[i].Points, Fill = new SolidColorBrush(hexagones[i].HexagoneColor) });
                }
                OnPropertyChanged("Hexagones");
            }
        }

        private void SaveFile(object obj)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".xml";
            saveFileDialog.FileName = "New_shapes.xml";
            saveFileDialog.Filter = "XML documents (.xml)|*.xml";
            if (saveFileDialog.ShowDialog() == true)
            {
                string fileName = saveFileDialog.FileName;
                List<Hexagone> hexagones = new List<Hexagone>();
                foreach (var elem in Hexagones)
                {
                    hexagones.Add(new Hexagone(elem));
                }
                using (Stream outputFile = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Hexagone>));
                    serializer.Serialize(outputFile, hexagones);
                }
            }
        }

        private void CloseWindow(object obj)
        {
            (obj as MainWindow).Close();
        }

        //Selecting and draging hexogones
        private void SelectHexagone(object obj)
        {
            Polygon curHexagone = (obj as Polygon);
            curHexagone.MouseDown += new MouseButtonEventHandler(Hexagone_MouseDown);
            OnPropertyChanged("Hexagones");
        }

        private void Drag(object obj)
        {
            Canvas plane = (obj as Canvas);
            plane.MouseMove += new MouseEventHandler(Canvas_MouseMove);
            plane.MouseUp += new MouseButtonEventHandler(Canvas_MouseUp);
        }

        //Events
        void Hexagone_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AllowDragging = true;
            SelectedHexagone = sender as Polygon;
            MousePosition = e.GetPosition(SelectedHexagone);
        }

        void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            AllowDragging = false;
        }

        void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (AllowDragging)
            {
                Canvas.SetLeft(SelectedHexagone, e.GetPosition(sender as IInputElement).X - MousePosition.X);
                Canvas.SetTop(SelectedHexagone, e.GetPosition(sender as IInputElement).Y - MousePosition.Y);
            }
        }
    }
}
