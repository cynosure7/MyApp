using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace MyApp.Controllers
{

    public class ProductController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ProductController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        // GET product
        [Route("[controller]")]
        public JsonResult Get()
        {
            try
            {
                var result = new JsonResult(JsonConvert.DeserializeObject(GetJsonFileData()));
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET product
        [Route("[controller]/{productCode}")]
        public JsonResult Get(string productCode)
        {
            JsonResult response = null;
            try
            {
                dynamic dynJson = JsonConvert.DeserializeObject(GetJsonFileData());
                foreach (var item in dynJson)
                {
                    if (item.code.ToString().Equals(productCode.ToUpper()))
                    {
                        response = new JsonResult(JsonConvert.DeserializeObject(item.ToString()));
                        break;
                    }
                }
                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private string GetJsonFileData()
        {
            var file = new DirectoryInfo(Path.GetFullPath(_hostingEnvironment.WebRootPath)).GetFiles("Product.json").FirstOrDefault();

            if (file != null && file.Exists)
            {
                string productJson;
                using (var stream = new StreamReader(file.FullName))
                {
                    productJson = stream.ReadToEnd();
                }
                return productJson;
            }

            return null;
        }
    }
}