using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Collections;

namespace PathEditor
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        const string SETTING_FILE = "setting.json";
        DispatcherTimer _dispatcherTimer;
        public MainWindow()
        {
            InitializeComponent();
            using (FileStream fs = new FileStream(SETTING_FILE, FileMode.Open))
            {
                try
                {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(CustomDs));
                    CustomDs ds = (CustomDs)ser.ReadObject(fs);
                    _prop.TextProp1 = ds.TextProp1;
                    _prop.TextProp2 = ds.TextProp2;
                    _prop.TextProp3 = ds.TextProp3;
                    _prop.TextProp4 = ds.TextProp4;
                    _prop.ConstText = ds.ConstText;
                }
                catch(Exception e)
                {
                    //
                }
            }
            _dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal);
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            _dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            _dispatcherTimer.Start();
        }
        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            _prop.TextProp1 = _textbox1.Text;
            _prop.TextProp2 = _textbox2.Text;
            _prop.TextProp3 = _textbox3.Text;
            _prop.TextProp4 = _textbox4.Text;
            _prop.ConstText = _const_text.Text;
            _prop.setSize(this.Height, this.Width);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            using (FileStream fs = new FileStream(SETTING_FILE, FileMode.Create))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(CustomDs));
                ser.WriteObject(fs, _prop);
            }
        }
    }

    [DataContract]
    public class CustomDs : INotifyPropertyChanged
    {
        Dictionary<string, string> _basic_table = new Dictionary<string, string>();
        Dictionary<string, string> _replace_table = new Dictionary<string, string>();
        const string HEIGHT = "[Height]";
        const string WIDTH = "[Width]";

        public void setSize(double h, double w)
        {
            if(_basic_table.ContainsKey(HEIGHT))
            {
                _basic_table[HEIGHT] = h.ToString();
            }
            else
            {
                _basic_table.Add(HEIGHT, h.ToString());
            }
            if(_basic_table.ContainsKey(WIDTH))
            {
                _basic_table[WIDTH] = w.ToString();
            }
            else
            {
                _basic_table.Add(WIDTH, w.ToString());
            }
            TextProp1 = TextProp1;
        }

        void setTable(string key, string val)
        {
            bool update = false;
            if (false == _replace_table.ContainsKey(key))
            {
                update = true;
                _replace_table.Add(key, val);
            }
            else
            {
                if (_replace_table[key] != val)
                {
                    update = true;
                }
                _replace_table[key] = val;
            }
        }

        string tableExchange()
        {
            string text = "";
            foreach (var table in _basic_table)
            {
                text += table.Key + "=" + table.Value + "\n";
            }
            foreach (var table in _replace_table)
            {
                text += table.Key + "=" + table.Value + "\n";
            }
            return text;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        string _textprop1 = "";
        public string TextProp1
        {
            get { return _textprop1; }
            set
            {
                SetProperty(ref _textprop1, value);
                TextProp1_A = textExchange(_textprop1);
            }
        }
        string _textprop1_a = "";
        public string TextProp1_A
        {
            get { return _textprop1_a; }
            set { SetProperty(ref _textprop1_a, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        string _textprop2_a = "";
        public string TextProp2_A
        {
            get { return _textprop2_a; }
            set { SetProperty(ref _textprop2_a, value); }
        }
        string _textprop2 = "";
        public string TextProp2
        {
            get { return _textprop2; }
            set
            {
                SetProperty(ref _textprop2, value);
                TextProp2_A = textExchange(_textprop2);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        string _textprop3_a = "";
        public string TextProp3_A
        {
            get { return _textprop3_a; }
            set { SetProperty(ref _textprop3_a, value); }
        }
        string _textprop3 = "";
        public string TextProp3
        {
            get { return _textprop3; }
            set
            {
                SetProperty(ref _textprop3, value);
                TextProp3_A = textExchange(_textprop3);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        string _textprop4_a = "";
        public string TextProp4_A
        {
            get { return _textprop4_a; }
            set { SetProperty(ref _textprop4_a, value); }
        }
        string _textprop4 = "";
        public string TextProp4
        {
            get { return _textprop4; }
            set
            {
                SetProperty(ref _textprop4, value);
                TextProp4_A = textExchange(_textprop4);
            }
        }
        #region  
        
        [DataMember]
        string _consttext ="";
        public string ConstText
        {
            get { return _consttext; }
            set
            {
                if(_consttext != value )
                {
                    listUpdate(value);
                }
                SetProperty(ref _consttext, value);
            }
        }
        void listUpdate(string text)
        {
            _replace_table.Clear();
            var line_list = text.Split('\n');
            foreach(string line in line_list)
            {
                try
                {
                    var ele = line.Split('=');
                    try
                    {
                        setTable(ele[0], ele[1]);
                    }
                    catch
                    {

                    }
                }
                catch
                {
                    break;
                }
            }
        }
        #endregion

        #region イベント発火
        string textExchange(string text)
        {
            foreach (var table in _basic_table)
            {
                text = text.Replace(table.Key, table.Value);
            }
            foreach(var table in _replace_table)
            {
                text = text.Replace(table.Key, table.Value);
            }
            return text;
        }

        protected void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, value))
                return;

            storage = value;
            NotifyPropertyChanged(propertyName);
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }

    [System.Windows.Data.ValueConversion(typeof(string), typeof(Geometry))]
    class GeometoryConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string string_value = (string)value;
            Geometry geometry;
            try
            {
                geometry = PathGeometry.Parse(string_value);
            }
            catch(Exception e)
            {
                geometry = Geometry.Parse("");
            }
            return geometry;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
