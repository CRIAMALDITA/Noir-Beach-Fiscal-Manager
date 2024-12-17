using RestaurantData.TablesDataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RestaurantData;
using RestaurantData.TablesDataClasses;

namespace RestaurantDataManager
{
    public class BussinessController
    {
        private string appDataPath;
        private string appDirectory;
        private string dataPath;
        private string defaultLogo;


        public BussinessController()
        {
            appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            appDirectory = Path.Combine(appDataPath, "CRIAMALDITA"); ;
            dataPath = Path.Combine(appDirectory, "BusinessData.json");
            defaultLogo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Business.png");
        }

        public BusinessData LoadData()
        {
            if (!Directory.Exists(appDirectory))
            {
                Directory.CreateDirectory(appDirectory);
            }
            else
            {
                if (!File.Exists(dataPath))
                {
                    BusinessData newData = GetDefaultData();
                    SaveData(newData);
                    return newData;
                }
            }
            try
            {
                string json = File.ReadAllText(dataPath);
                BusinessData loadedData = JsonSerializer.Deserialize<BusinessData>(json);
                return loadedData;
            }
            catch (Exception ex)
            {
                return GetDefaultData();
            }

        }
        public void SaveData(BusinessData data)
        {
            try
            {
                string json = JsonSerializer.Serialize(data);
                File.WriteAllText(dataPath, json, Encoding.UTF8);
            }
            catch (Exception ex)
            {
            }

        }
        public BusinessData GetDefaultData()
        {
            BusinessData newData = new BusinessData()
            {
                BusinessName = "Business",
                Tin = "123456789",
                Address = "Here",
                ImageURL = defaultLogo,
            };
            return newData;
        }
    }
}
