using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel mvm = new MainViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = mvm;
            GetData();
        }

        public void GetData()
        {
            string airport = "kgtu";
            string api = $"https://aviationweather.gov/adds/dataserver_current/httpparam?dataSource=metars&requestType=retrieve&format=xml&stationString={airport}&hoursBeforeNow=4";

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.Load(api);
            string jsonText = JsonConvert.SerializeXmlNode(doc);
            var obj = JsonConvert.DeserializeObject(jsonText).ToString();

            JObject googleSearch = JObject.Parse(obj);

            IList<JToken> results = googleSearch["response"]["data"]["METAR"].ToList();

            // serialize JSON results into .NET objects
            IList<Metar> searchResults = new List<Metar>();
            foreach (JToken result in results)
            {
                // JToken.ToObject is a helper method that uses JsonSerializer internally
                var searchResult = result.ToObject<Metar>();
                mvm.Metars.Add(searchResult);
                searchResults.Add(searchResult);
            }
        }
    }

    public class MainViewModel
    {
        ObservableCollection<Metar> metar = new ObservableCollection<Metar>();
        public ObservableCollection<Metar> Metars { get; set; } = new ObservableCollection<Metar>();
    }

    public class Metar
    {
        public string station_id { get; set; }
        public string raw_text { get; set; }
    }


}
