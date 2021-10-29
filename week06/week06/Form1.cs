using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using week06.Entities;
using week06.MnbServiceReference;

namespace week06
{
    public partial class Form1 : Form
    {

        BindingList<RateData> Rates = new BindingList<RateData>();
        BindingList<string> currencies = new BindingList<string>();

        public Form1()
        {
            InitializeComponent();
            comboBox1.DataSource = currencies;
            RefreshData();

            var mnbService = new MNBArfolyamServiceSoapClient();

            var request = new GetCurrenciesRequestBody();
            var response = mnbService.GetCurrencies(request);

            var result = response.GetCurrenciesResult;
            XmlDocument vxml = new XmlDocument();
            vxml.LoadXml(result);
            foreach (XmlElement element in vxml.DocumentElement.FirstChild.ChildNodes)
            {
                currencies.Add(element.InnerText);
            }
        }

        private void RefreshData()
        {
            if (comboBox1.SelectedItem == null)
                return;

            string xmlstring = Consume();
            LoadXml(xmlstring);
            dataGridView1.DataSource = Rates;
            Charting();
        }

        private void Charting()
        {
            chartRateData.DataSource = Rates;

            var series = chartRateData.Series[0];
            series.ChartType = SeriesChartType.Line;
            series.XValueMember = "Date";
            series.YValueMembers = "Value";
            series.BorderWidth = 2;

            var legend = chartRateData.Legends[0];
            legend.Enabled = false;

            var chartArea = chartRateData.ChartAreas[0];
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.IsStartedFromZero = false;
        }

        string Consume()
        {

            var mnbService = new MNBArfolyamServiceSoapClient();

            var request = new GetExchangeRatesRequestBody()
            {
                currencyNames = comboBox1.SelectedItem.ToString(),
                startDate = dateTimePicker1.Value.ToString("yyyy-MM-dd"),
                endDate = dateTimePicker2.Value.ToString("yyyy-MM-dd")
            };

            var response = mnbService.GetExchangeRates(request);

            var result = response.GetExchangeRatesResult;
            return result;
        }

        private void LoadXml(string input)
        {
            var xml = new XmlDocument();
            xml.LoadXml(input);

            foreach (XmlElement element in xml.DocumentElement)
            {
                var rate = new RateData();
                Rates.Add(rate);

                rate.Date = DateTime.Parse(element.GetAttribute("date"));

                var childElement = (XmlElement)element.ChildNodes[0];
                rate.Currency = childElement.GetAttribute("curr");

                var unit = decimal.Parse(childElement.GetAttribute("unit"));
                var value = decimal.Parse(childElement.InnerText);
                if (unit != 0)
                    rate.Value = value / unit;
            }
        }

        private void filterChanged(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}
