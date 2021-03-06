﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDBCreate
{
    public class ProductDB
    {
        [BsonId]
        public ObjectId ID { get; set; }
        public int op { get; set; }
        public string Uriname { get; set; }
        public string ProductName { get; set; }
        public string CompanyShort { get; set; }
        public string Farmer { get; set; }
        public string Origin { get; set; }
        public string PackedDate { get; set; }
        public string VarifiedCompany { get; set; }
        public string dishName1 { get; set; }
        public string dishPhoto1 { get; set; }
        public string dishUrl1 { get; set; }
        public string dishName2 { get; set; }
        public string dishPhoto2 { get; set; }
        public string dishUrl2 { get; set; }
        public string dishName3 { get; set; }
        public string dishPhoto3 { get; set; }
        public string dishUrl3 { get; set; }
        public string dishName4 { get; set; }
        public string dishPhoto4 { get; set; }
        public string dishUrl4 { get; set; }
    }
}

namespace webscraping {
    public partial class Form1 : Form {
        HtmlWeb client = new HtmlWeb();

        public Form1() {
            InitializeComponent();
        }

        private async Task<int> ProductionRecord (string url, Resume[] resume) {
            var doc = await Task.Factory.StartNew(() => client.Load(url));
 
            var dateNodes = doc.DocumentNode.SelectNodes("//*[@id=\"tableSort\"]//tr/td[1]");
            var typeNodes = doc.DocumentNode.SelectNodes("//*[@id=\"tableSort\"]//tr/td[2]");
            var contentNodes = doc.DocumentNode.SelectNodes("//*[@id=\"tableSort\"]//tr/td[3]");
            var refNodes = doc.DocumentNode.SelectNodes("//*[@id=\"tableSort\"]//tr//td[4]");

            if (dateNodes == null || typeNodes == null || contentNodes == null) {
                return -1;
            }

            var innerDate = dateNodes.Select(node => node.InnerText).ToList();
            var innerTypes = typeNodes.Select(node => node.InnerText).ToList();
            var innerContent = contentNodes.Select(node => node.InnerText).ToList();
            var innerRef = refNodes.Select(node => node.InnerText).ToList();

            int cnt = innerDate.Count();

            for (int i = 0; i < innerDate.Count(); ++i) {
                resume[i].Date = innerDate[i];
                resume[i].Type = innerTypes[i];
                resume[i].Content = innerContent[i];
                resume[i].Ref = innerRef[i];
            }

            return cnt;
        }

        private async Task<Boolean> FarmRecord (string url, Product[] pro)
        {
            var doc = await Task.Factory.StartNew(() => client.Load(url));

            var companyShort = doc.GetElementbyId("ctl00_ContentPlaceHolder1_Producer").InnerText;
            var Farmer = doc.GetElementbyId("ctl00_ContentPlaceHolder1_FarmerName").InnerText;
            var productName = doc.GetElementbyId("ctl00_ContentPlaceHolder1_ProductName").InnerText;
            var origin = doc.GetElementbyId("ctl00_ContentPlaceHolder1_Place").InnerText;
            var packedDate = doc.GetElementbyId("ctl00_ContentPlaceHolder1_PackDate").InnerText;
            var varifiedCompany = doc.GetElementbyId("ctl00_ContentPlaceHolder1_ao_name").InnerText;

            if (companyShort == null) {
                return false;
            }
            
            pro[0].CompanyShort = companyShort;
            pro[0].Farmer = Farmer;
            pro[0].ProductName = productName;
            pro[0].Origin = origin;
            pro[0].PackedDate = packedDate;
            pro[0].VarifiedCompany = varifiedCompany;

            return true;
        }

        private async Task<Boolean> getFurtherInfo (string url, Recipe[] re)
        {
            var doc = await Task.Factory.StartNew(() => client.Load(url));
            
            var nulltest = doc.DocumentNode.SelectSingleNode("//*[@id=\"ctl00_ContentPlaceHolder1_RecommandDIV\"]/div/ul/li[1]/p/a");

            if (nulltest != null) {
                re[0].dishName = doc.DocumentNode.SelectSingleNode("//*[@id=\"ctl00_ContentPlaceHolder1_RecommandDIV\"]/div/ul/li[1]/p/a").InnerText.ToString();
                re[0].dishPhoto = doc.DocumentNode.SelectSingleNode("//*[@id=\"ctl00_ContentPlaceHolder1_RecommandDIV\"]/div/ul/li[1]/div/a/img").Attributes["src"].Value;
                re[0].dishUrl = "https://taft.coa.gov.tw" + doc.DocumentNode.SelectSingleNode("//*[@id=\"ctl00_ContentPlaceHolder1_RecommandDIV\"]/div/ul/li[1]/p/a").Attributes["href"].Value;

                re[1].dishName = doc.DocumentNode.SelectSingleNode("//*[@id=\"ctl00_ContentPlaceHolder1_RecommandDIV\"]/div/ul/li[2]/p/a").InnerText.ToString();
                re[1].dishPhoto = doc.DocumentNode.SelectSingleNode("//*[@id=\"ctl00_ContentPlaceHolder1_RecommandDIV\"]/div/ul/li[2]/div/a/img").Attributes["src"].Value;
                re[1].dishUrl = "https://taft.coa.gov.tw" + doc.DocumentNode.SelectSingleNode("//*[@id=\"ctl00_ContentPlaceHolder1_RecommandDIV\"]/div/ul/li[2]/p/a").Attributes["href"].Value;

                re[2].dishName = doc.DocumentNode.SelectSingleNode("//*[@id=\"ctl00_ContentPlaceHolder1_RecommandDIV\"]/div/ul/li[3]/p/a").InnerText.ToString();
                re[2].dishPhoto = doc.DocumentNode.SelectSingleNode("//*[@id=\"ctl00_ContentPlaceHolder1_RecommandDIV\"]/div/ul/li[3]/div/a/img").Attributes["src"].Value;
                re[2].dishUrl = "https://taft.coa.gov.tw" + doc.DocumentNode.SelectSingleNode("//*[@id=\"ctl00_ContentPlaceHolder1_RecommandDIV\"]/div/ul/li[3]/p/a").Attributes["href"].Value;

                re[3].dishName = doc.DocumentNode.SelectSingleNode("//*[@id=\"ctl00_ContentPlaceHolder1_RecommandDIV\"]/div/ul/li[4]/p/a").InnerText.ToString();
                re[3].dishPhoto = doc.DocumentNode.SelectSingleNode("//*[@id=\"ctl00_ContentPlaceHolder1_RecommandDIV\"]/div/ul/li[4]/div/a/img").Attributes["src"].Value;
                re[3].dishUrl = "https://taft.coa.gov.tw" + doc.DocumentNode.SelectSingleNode("//*[@id=\"ctl00_ContentPlaceHolder1_RecommandDIV\"]/div/ul/li[4]/p/a").Attributes["href"].Value;

                return true;
            } else {
                return false;
            }
        }

        private async Task<string> getPediaUrl (string url) {
            var doc = await Task.Factory.StartNew(() => client.Load(url));

            var nodetest = doc.DocumentNode.SelectSingleNode("//*[@id=\"ctl00_ContentPlaceHolder1_ProductName\"]/a");

            if (nodetest != null) {
                string toReturn = doc.DocumentNode.SelectSingleNode("//*[@id=\"ctl00_ContentPlaceHolder1_ProductName\"]/a").Attributes["href"].Value;
                return toReturn;
            } else {
                return "NULL";
            }
        }

        private async void Form1_Load(object sender, EventArgs e) {
            string url = "https://taft.coa.gov.tw/rsm/Code_cp.aspx?ID=1561424&EnTraceCode=10609030565";
            // MongoDB Setup
            var connectionString = "mongodb://msp12:msp2017@ds123084.mlab.com:23084/msp";
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase("msp");
            IMongoCollection<MongoDBCreate.ProductDB> collection = db.GetCollection<MongoDBCreate.ProductDB>("productResume");

            Recipe[] topRecipe = new Recipe[4];
            Product[] ProductInfo = new Product[1];
            Resume[] ProductResume = new Resume[100];

            var resumecnt = await ProductionRecord(url, ProductResume);
            
            if (resumecnt != -1) {
                for (int i = 0; i < resumecnt; ++i) {
                    Console.WriteLine(ProductResume[i].Date);
                    Console.WriteLine(ProductResume[i].Type);
                    Console.WriteLine(ProductResume[i].Content);
                    Console.WriteLine(ProductResume[i].Ref);
                    Console.WriteLine("****************");
                }
            } else {
                Console.WriteLine("Errors in parsing records");
            }

            var farmresults = await FarmRecord(url, ProductInfo);

            if (farmresults) {
                Console.WriteLine(ProductInfo[0].CompanyShort);
                Console.WriteLine(ProductInfo[0].Farmer);
                Console.WriteLine(ProductInfo[0].Origin);
                Console.WriteLine(ProductInfo[0].PackedDate);
                Console.WriteLine(ProductInfo[0].ProductName);
                Console.WriteLine(ProductInfo[0].VarifiedCompany);
            } else {
                Console.WriteLine("Errors in parsing farmresults");
            }

            bool hasRecipe = await getFurtherInfo(url, topRecipe);

            if (hasRecipe) {
                for (int i = 0; i < 4; ++i) {
                    Console.WriteLine(topRecipe[i].dishName);
                    Console.WriteLine(topRecipe[i].dishPhoto);
                    Console.WriteLine(topRecipe[i].dishUrl);
                    Console.WriteLine("------------------");
                }
            } else {
                Console.WriteLine("No recipe found");
            }

            var pediaUrl = await getPediaUrl(url);

            if (pediaUrl != "NULL") {
                Console.WriteLine(pediaUrl);
            } else {
                Console.WriteLine("No URL found");
            }

            //Decode picture and update url
            var filter = Builders<MongoDBCreate.ProductDB>.Filter.Eq("op", 1);

            var update = Builders<MongoDBCreate.ProductDB>.Update
                .Set("Uriname", url).Set("ProductName", ProductInfo[0].ProductName)
                .Set("CompanyShort", ProductInfo[0].CompanyShort).Set("Farmer", ProductInfo[0].Farmer)
                .Set("Origin", ProductInfo[0].Origin).Set("PackedDate", ProductInfo[0].PackedDate)
                .Set("dishName1", topRecipe[0].dishName)
                .Set("dishPhoto1", topRecipe[0].dishPhoto)
                .Set("dishUrl1", topRecipe[0].dishUrl)
                .Set("dishName2", topRecipe[1].dishName)
                .Set("dishPhoto2", topRecipe[1].dishPhoto)
                .Set("dishUrl2", topRecipe[1].dishUrl)
                .Set("dishName3", topRecipe[2].dishName)
                .Set("dishPhoto3", topRecipe[2].dishPhoto)
                .Set("dishUrl3", topRecipe[2].dishUrl)
                .Set("dishName4", topRecipe[3].dishName)
                .Set("dishPhoto4", topRecipe[3].dishPhoto)
                .Set("dishUrl4", topRecipe[3].dishUrl);

            var result = await collection.UpdateOneAsync(filter, update);

            //find last decoded url
            var user = collection.Find(r => r.op == 1).Limit(1).ToList();
        }
    }
}