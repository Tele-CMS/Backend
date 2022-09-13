using HC.Common;
using HC.Common.HC.Common;
using HC.Patient.Entity;
using HC.Patient.Model;
using HC.Patient.Model.Image;
using HC.Patient.Service.IServices.Images;
using HC.Service;
using Svg;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace HC.Patient.Service.Services.Images
{
    public class ImageService : BaseService, IImageService
    {
        /// <summary>
        /// it will save organization's logo and favicon images
        /// </summary>
        /// <param name="base64String"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public string SaveImages(string base64String,string directory, string folderName)
        {
            //get current directory root
            string webRootPath = Directory.GetCurrentDirectory();

            //add your custom path
            webRootPath = webRootPath + directory + folderName;

            //check 
            if (!Directory.Exists(webRootPath))
            {
                Directory.CreateDirectory(webRootPath);
            }

            if (!string.IsNullOrEmpty(base64String))
            {
                //getting data from base64 url
                var base64Data = Regex.Match(base64String, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                //getting extension of the image
                string extension = CommonMethods.GetExtenstion(Regex.Match(base64String, @"data:(?<type>.+?),(?<data>.+)").Groups["type"].Value.Split(';')[0]);

                //file name
                string fileName = folderName + "_" + DateTime.UtcNow.TimeOfDay.ToString();

                //update file name remove unsupported attr.
                fileName = fileName.Replace(" ", "_").Replace(":", "_");

                //add extension
                if (!extension.StartsWith("."))
                {
                    extension = "." + extension;
                }
                fileName = fileName + extension;

                //create path for save location
                string path = webRootPath + "/" + fileName;

                //convert files into base
                byte[] bytes = Convert.FromBase64String(base64Data);

                TypeImageModel typeImageModel = new TypeImageModel();
                typeImageModel.Type = folderName;
                typeImageModel.Url = path;
                typeImageModel.Bytes = bytes;

                ////save into the directory
                CommonMethods.SaveImages(typeImageModel);
                //File.WriteAllBytes(path, bytes);

                return fileName;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Save patient images
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commonMethods"></param>
        /// <returns></returns>
        public Patients ConvertBase64ToImage(Patients entity)
        {
            try
            {
                if (!string.IsNullOrEmpty(entity.PhotoBase64))
                {
                    string webRootPath = "";

                    //get root path
                    webRootPath = Directory.GetCurrentDirectory();

                    //getting data from base64 url
                    var base64Data = Regex.Match(entity.PhotoBase64, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;

                    //getting extension of the image
                    string extension = Regex.Match(entity.PhotoBase64, @"data:image/(?<type>.+?),(?<data>.+)").Groups["type"].Value.Split(';')[0];

                    extension = "." + extension;

                    if (!Directory.Exists(webRootPath + ImagesPath.PatientPhotos))
                    {
                        Directory.CreateDirectory(webRootPath + ImagesPath.PatientPhotos);
                    }
                    if (!Directory.Exists(webRootPath + ImagesPath.PatientThumbPhotos))
                    {
                        Directory.CreateDirectory(webRootPath + ImagesPath.PatientThumbPhotos);
                    }

                    string picName = Guid.NewGuid().ToString();

                    List<ImageModel> obj = new List<ImageModel>();

                    ImageModel img = new ImageModel();

                    img.Base64 = base64Data;
                    img.ImageUrl = webRootPath + ImagesPath.PatientPhotos + picName + extension;
                    img.ThumbImageUrl = webRootPath + ImagesPath.PatientThumbPhotos + picName + extension;
                    obj.Add(img);

                    CommonMethods.SaveImageAndThumb(obj);

                    entity.PhotoPath = picName + extension;
                    entity.PhotoThumbnailPath = picName + extension;

                }
                else if (string.IsNullOrEmpty(entity.PhotoPath) && string.IsNullOrEmpty(entity.PhotoThumbnailPath))
                {
                    entity.PhotoPath = string.Empty;
                    entity.PhotoThumbnailPath = string.Empty;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return entity;
        }

        /// <summary>
        /// Save user(staff) images
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commonMethods"></param>
        /// <returns></returns>
        public Staffs ConvertBase64ToImageForUser(Staffs entity)
        {
            try
            {
                if (!string.IsNullOrEmpty(entity.PhotoBase64))
                {
                    string webRootPath = "";

                    //get root path
                    webRootPath = Directory.GetCurrentDirectory();

                    //getting data from base64 url
                    var base64Data = Regex.Match(entity.PhotoBase64, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;

                    //getting extension of the image
                    string extension = Regex.Match(entity.PhotoBase64, @"data:image/(?<type>.+?),(?<data>.+)").Groups["type"].Value.Split(';')[0];

                    extension = "." + extension;

                    if (!Directory.Exists(webRootPath + ImagesPath.StaffPhotos))
                    {
                        Directory.CreateDirectory(webRootPath + ImagesPath.StaffPhotos);
                    }
                    if (!Directory.Exists(webRootPath + ImagesPath.StaffThumbPhotos))
                    {
                        Directory.CreateDirectory(webRootPath + ImagesPath.StaffThumbPhotos);
                    }

                    string picName = Guid.NewGuid().ToString();

                    List<ImageModel> obj = new List<ImageModel>();

                    ImageModel img = new ImageModel();

                    img.Base64 = base64Data;
                    img.ImageUrl = webRootPath + ImagesPath.StaffPhotos + picName + extension;
                    img.ThumbImageUrl = webRootPath + ImagesPath.StaffThumbPhotos + picName + extension;
                    obj.Add(img);

                    CommonMethods.SaveImageAndThumb(obj);

                    entity.PhotoPath = picName + extension;
                    entity.PhotoThumbnailPath = picName + extension;

                }
                else if (string.IsNullOrEmpty(entity.PhotoPath) && string.IsNullOrEmpty(entity.PhotoThumbnailPath))
                {
                    entity.PhotoPath = string.Empty;
                    entity.PhotoThumbnailPath = string.Empty;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return entity;
        }

        /// <summary>
        /// Save patient insurance detail images
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commonMethods"></param>
        /// 


        /// <summary>
        /// Save Speciality(GlobalCode) images
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commonMethods"></param>
        /// <returns></returns>
        public GlobalCodeModel ConvertBase64ToImageForDocSpeciality(GlobalCodeModel entity)
        {
            try
            {
                if (!string.IsNullOrEmpty(entity.SpecialityIcon))
                {
                    string webRootPath = "";

                    //get root path
                    webRootPath = Directory.GetCurrentDirectory();

                    //getting data from base64 url
                    var base64Data = Regex.Match(entity.SpecialityIcon, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;

                    //getting extension of the image
                    string extension = Regex.Match(entity.SpecialityIcon, @"data:image/(?<type>.+?),(?<data>.+)").Groups["type"].Value.Split(';')[0];
                    if (extension == "svg+xml")
                    {

                           extension = ".png";

                        // var svgContent = "PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI1NiIgaGVpZ2h0PSI1NiIgdmlld0JveD0iMCAwIDU2IDU2Ij4KICAgIDxwYXRoIGZpbGw9IiNENUQ1RDgiIHN0cm9rZT0iI0Q1RDVEOCIgc3Ryb2tlLXdpZHRoPSIxLjM1IiBkPSJNNDguMTY3IDQ1LjkyNmM5LjA5NS0xMC4xOTcgOS4xMTItMjUuNTg4LjA0Mi0zNS44MDVhMS40NDQgMS40NDQgMCAwMC0uMTctLjIgMjYuOTU4IDI2Ljk1OCAwIDAwLTQwLjA3MiAwYy0uMDY2LjA2LS4xMjUuMTI3LS4xNzYuMi05LjA1NSAxMC4yLTkuMDU1IDI1LjU1OCAwIDM1Ljc1OC4wNS4wNzEuMTA4LjEzOC4xNy4yYTI2Ljk2NCAyNi45NjQgMCAwMDQwLjA3NyAwYy4wNDYtLjA0OS4wOS0uMS4xMy0uMTUzek0zLjM3NyAyOS4xNzRoMTAuNTM2Yy4wNjUgNC4wODcuNjU3IDguMTUgMS43NiAxMi4wODVhMzMuMjYyIDMzLjI2MiAwIDAwLTYuNjMyIDIuNDc3IDI0LjUxMiAyNC41MTIgMCAwMS01LjY2NC0xNC41NjJoMHptNS42NjQtMTYuOTFhMzMuMjYyIDMzLjI2MiAwIDAwNi42MzMgMi40NzYgNDcuNTQ0IDQ3LjU0NCAwIDAwLTEuNzYxIDEyLjA4NkgzLjM0OEEyNC41MTIgMjQuNTEyIDAgMDE5LjA0IDEyLjI2M2gwem00My42MSAxNC41NjJINDIuMDg4YTQ3LjU0NCA0Ny41NDQgMCAwMC0xLjc2MS0xMi4wODYgMzMuMjYyIDMzLjI2MiAwIDAwNi42MzItMi40NzcgMjQuNTEyIDI0LjUxMiAwIDAxNS42OTQgMTQuNTYzek0yNi44MjcgMTMuOTEzYTQ2LjUwMyA0Ni41MDMgMCAwMS04LjA3Ny0uODU3YzEuOTM3LTUuMjQyIDQuODM3LTguODcgOC4wNzctOS41OHYxMC40Mzd6bS04LjgwNCAxLjM2N2MyLjg5OS41OTEgNS44NDYuOTIgOC44MDQuOTh2MTAuNTY2SDE2LjI2Yy4wOC0zLjkwOS42NzItNy43OSAxLjc2LTExLjU0Nmgwem04LjgwNCAxMy44OTR2MTAuNTY1YTQ5LjE1IDQ5LjE1IDAgMDAtOC44MDQuOTYzIDQ0Ljc2OCA0NC43NjggMCAwMS0xLjc2MS0xMS41MjhoMTAuNTY1em0wIDEyLjkxM3YxMC40MzZjLTMuMjQtLjcxLTYuMTQtNC4zMzgtOC4wNzctOS41NjJhNDYuNTAzIDQ2LjUwMyAwIDAxOC4wNzctLjg3NGgwem0yLjM0OCAwYzIuNzEyLjA1IDUuNDE0LjMzNiA4LjA3Ni44NTctMS45MzcgNS4yMjQtNC44MzYgOC44NTEtOC4wNzYgOS41NjFWNDIuMDg3em04LjgwNC0xLjM4NWE0OS4xNSA0OS4xNSAwIDAwLTguODA0LS45NjNWMjkuMTc0aDEwLjU2NWE0NC43NjggNDQuNzY4IDAgMDEtMS43NjEgMTEuNTQ1di0uMDE3em0tOC44MDQtMTMuODc2VjE2LjI2YTQ5LjE1IDQ5LjE1IDAgMDA4LjgwNC0uOTYzIDQ0Ljc2OCA0NC43NjggMCAwMTEuNzYgMTEuNTI4SDI5LjE3NXptMC0xMi45MTNWMy40NzdjMy4yNC43MSA2LjE0IDQuMzM3IDguMDc2IDkuNTYxLTIuNjYxLjUyNy01LjM2NC44Mi04LjA3Ni44NzVoMHptMTAuNDI0LTEuMzk3YTIzLjIyNiAyMy4yMjYgMCAwMC00LjcyNS04LjE4OCAyNC42NTIgMjQuNjUyIDAgMDExMC40MyA2LjEzMyAzMS44MTIgMzEuODEyIDAgMDEtNS43MDUgMi4wMzd2LjAxOHptLTIzLjE5NiAwYTMxLjgxMiAzMS44MTIgMCAwMS01LjcwNS0yLjAzN2MyLjkxLTIuODg2IDYuNDk2LTUgMTAuNDMtNi4xNTFhMjMuMjI2IDIzLjIyNiAwIDAwLTQuNzI1IDguMTd2LjAxOHptMCAzMS4wMDNjLjk5OSAzLjAxOCAyLjYwNyA1LjggNC43MjUgOC4xN2EyNC42NTIgMjQuNjUyIDAgMDEtMTAuNDMtNi4xNSAzMS44MTIgMzEuODEyIDAgMDE1LjcwNS0yLjAzOHYuMDE4em0yMy4xOTYgMGEzMS44MTIgMzEuODEyIDAgMDE1LjcwNSAyLjAzNyAyNC42NTIgMjQuNjUyIDAgMDEtMTAuNDMgNi4xMTYgMjMuMjI2IDIzLjIyNiAwIDAwNC43MjUtOC4xN3YuMDE3em0uNzI4LTIuMjZhNDcuNTQ0IDQ3LjU0NCAwIDAwMS43Ni0xMi4wODVoMTAuNTY2YTI0LjUxMiAyNC41MTIgMCAwMS01LjY5NCAxNC41NjIgMzMuMjYyIDMzLjI2MiAwIDAwLTYuNjMyLTIuNDc3eiIvPgo8L3N2Zz4K";
                        var svgContent = "PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI1NiIgaGVpZ2h0PSI1NiIgdmlld0JveD0iMCAwIDU2IDU2Ij4KICAgIDxwYXRoIGZpbGw9IiNENUQ1RDgiIHN0cm9rZT0iI0Q1RDVEOCIgc3Ryb2tlLXdpZHRoPSIxLjM1IiBkPSJNNDguMTY3IDQ1LjkyNmM5LjA5NS0xMC4xOTcgOS4xMTItMjUuNTg4LjA0Mi0zNS44MDVhMS40NDQgMS40NDQgMCAwMC0uMTctLjIgMjYuOTU4IDI2Ljk1OCAwIDAwLTQwLjA3MiAwYy0uMDY2LjA2LS4xMjUuMTI3LS4xNzYuMi05LjA1NSAxMC4yLTkuMDU1IDI1LjU1OCAwIDM1Ljc1OC4wNS4wNzEuMTA4LjEzOC4xNy4yYTI2Ljk2NCAyNi45NjQgMCAwMDQwLjA3NyAwYy4wNDYtLjA0OS4wOS0uMS4xMy0uMTUzek0zLjM3NyAyOS4xNzRoMTAuNTM2Yy4wNjUgNC4wODcuNjU3IDguMTUgMS43NiAxMi4wODVhMzMuMjYyIDMzLjI2MiAwIDAwLTYuNjMyIDIuNDc3IDI0LjUxMiAyNC41MTIgMCAwMS01LjY2NC0xNC41NjJoMHptNS42NjQtMTYuOTFhMzMuMjYyIDMzLjI2MiAwIDAwNi42MzMgMi40NzYgNDcuNTQ0IDQ3LjU0NCAwIDAwLTEuNzYxIDEyLjA4NkgzLjM0OEEyNC41MTIgMjQuNTEyIDAgMDE5LjA0IDEyLjI2M2gwem00My42MSAxNC41NjJINDIuMDg4YTQ3LjU0NCA0Ny41NDQgMCAwMC0xLjc2MS0xMi4wODYgMzMuMjYyIDMzLjI2MiAwIDAwNi42MzItMi40NzcgMjQuNTEyIDI0LjUxMiAwIDAxNS42OTQgMTQuNTYzek0yNi44MjcgMTMuOTEzYTQ2LjUwMyA0Ni41MDMgMCAwMS04LjA3Ny0uODU3YzEuOTM3LTUuMjQyIDQuODM3LTguODcgOC4wNzctOS41OHYxMC40Mzd6bS04LjgwNCAxLjM2N2MyLjg5OS41OTEgNS44NDYuOTIgOC44MDQuOTh2MTAuNTY2SDE2LjI2Yy4wOC0zLjkwOS42NzItNy43OSAxLjc2LTExLjU0Nmgwem04LjgwNCAxMy44OTR2MTAuNTY1YTQ5LjE1IDQ5LjE1IDAgMDAtOC44MDQuOTYzIDQ0Ljc2OCA0NC43NjggMCAwMS0xLjc2MS0xMS41MjhoMTAuNTY1em0wIDEyLjkxM3YxMC40MzZjLTMuMjQtLjcxLTYuMTQtNC4zMzgtOC4wNzctOS41NjJhNDYuNTAzIDQ2LjUwMyAwIDAxOC4wNzctLjg3NGgwem0yLjM0OCAwYzIuNzEyLjA1IDUuNDE0LjMzNiA4LjA3Ni44NTctMS45MzcgNS4yMjQtNC44MzYgOC44NTEtOC4wNzYgOS41NjFWNDIuMDg3em04LjgwNC0xLjM4NWE0OS4xNSA0OS4xNSAwIDAwLTguODA0LS45NjNWMjkuMTc0aDEwLjU2NWE0NC43NjggNDQuNzY4IDAgMDEtMS43NjEgMTEuNTQ1di0uMDE3em0tOC44MDQtMTMuODc2VjE2LjI2YTQ5LjE1IDQ5LjE1IDAgMDA4LjgwNC0uOTYzIDQ0Ljc2OCA0NC43NjggMCAwMTEuNzYgMTEuNTI4SDI5LjE3NXptMC0xMi45MTNWMy40NzdjMy4yNC43MSA2LjE0IDQuMzM3IDguMDc2IDkuNTYxLTIuNjYxLjUyNy01LjM2NC44Mi04LjA3Ni44NzVoMHptMTAuNDI0LTEuMzk3YTIzLjIyNiAyMy4yMjYgMCAwMC00LjcyNS04LjE4OCAyNC42NTIgMjQuNjUyIDAgMDExMC40MyA2LjEzMyAzMS44MTIgMzEuODEyIDAgMDEtNS43MDUgMi4wMzd2LjAxOHptLTIzLjE5NiAwYTMxLjgxMiAzMS44MTIgMCAwMS01LjcwNS0yLjAzN2MyLjkxLTIuODg2IDYuNDk2LTUgMTAuNDMtNi4xNTFhMjMuMjI2IDIzLjIyNiAwIDAwLTQuNzI1IDguMTd2LjAxOHptMCAzMS4wMDNjLjk5OSAzLjAxOCAyLjYwNyA1LjggNC43MjUgOC4xN2EyNC42NTIgMjQuNjUyIDAgMDEtMTAuNDMtNi4xNSAzMS44MTIgMzEuODEyIDAgMDE1LjcwNS0yLjAzOHYuMDE4em0yMy4xOTYgMGEzMS44MTIgMzEuODEyIDAgMDE1LjcwNSAyLjAzNyAyNC42NTIgMjQuNjUyIDAgMDEtMTAuNDMgNi4xMTYgMjMuMjI2IDIzLjIyNiAwIDAwNC43MjUtOC4xN3YuMDE3em0uNzI4LTIuMjZhNDcuNTQ0IDQ3LjU0NCAwIDAwMS43Ni0xMi4wODVoMTAuNTY2YTI0LjUxMiAyNC41MTIgMCAwMS01LjY5NCAxNC41NjIgMzMuMjYyIDMzLjI2MiAwIDAwLTYuNjMyLTIuNDc3eiIvPgo8L3N2Zz4K";
                      //  var svgContent = "PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiIHN0YW5kYWxvbmU9Im5vIj8+PCFET0NUWVBFIHN2ZyBQVUJMSUMgIi0vL1czQy8vRFREIFNWRyAxLjEvL0VOIiAiaHR0cDovL3d3dy53My5vcmcvR3JhcGhpY3MvU1ZHLzEuMS9EVEQvc3ZnMTEuZHRkIj48c3ZnIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgdmVyc2lvbj0iMS4xIiB3aWR0aD0iNDAzIiBoZWlnaHQ9IjEwNiI+PHBhdGggZmlsbD0ibm9uZSIgc3Ryb2tlPSIjMDAwMDAwIiBzdHJva2Utd2lkdGg9IjIiIHN0cm9rZS1saW5lY2FwPSJyb3VuZCIgc3Ryb2tlLWxpbmVqb2luPSJyb3VuZCIgZD0iTSAxMjUgMSBjIC0wLjEyIDAuNjggLTMuOSAyNi4xOSAtNyAzOSBjIC0xLjkgNy44NyAtNC44NiAxNS44MiAtOCAyMyBjIC0xLjQgMy4xOSAtMy42MyA2Ljc4IC02IDkgYyAtMi41NiAyLjQgLTYuNTggNC45IC0xMCA2IGMgLTUuMzUgMS43MiAtMTEuOTYgMS42MiAtMTggMyBjIC0xMC4yNyAyLjM1IC0xOS43MSA1LjcxIC0zMCA4IGMgLTguMDYgMS43OSAtMTYuMyAzLjI4IC0yNCA0IGMgLTIuNTUgMC4yNCAtNi42MSAwLjM5IC04IC0xIGMgLTIuOTMgLTIuOTMgLTYuNzMgLTkuOTEgLTggLTE1IGMgLTIuNDkgLTkuOTYgLTMuMDggLTIxLjcyIC00IC0zMyBjIC0wLjc4IC05LjUxIC0xLjI2IC0xOS4zMSAtMSAtMjggYyAwLjA2IC0xLjk2IDEuMDggLTQuMzUgMiAtNiBjIDAuNjEgLTEuMTEgMS45MSAtMi41MiAzIC0zIGMgMS41NiAtMC42OSA0LjE1IC0wLjQ1IDYgLTEgYyAxLjM2IC0wLjQxIDIuNjUgLTEuODcgNCAtMiBjIDExLjUyIC0xLjEgMjYuMzkgLTIuNDcgMzggLTIgYyAzLjU3IDAuMTUgOC4wNCAxLjk5IDExIDQgYyA0LjgzIDMuMjggOS4zNyA4LjM3IDE0IDEzIGMgNi4zNSA2LjM1IDExLjY4IDEyLjg5IDE4IDE5IGMgMy40MyAzLjMyIDcuNDcgNS43MiAxMSA5IGMgNS45NyA1LjU0IDEwLjk4IDExLjUzIDE3IDE3IGMgNS4xNiA0LjY5IDEwLjM0IDkuNDggMTYgMTMgYyA2LjM5IDMuOTcgMTQuNDYgNi4xNSAyMSAxMCBjIDQuNjIgMi43MSA4LjQ3IDcuNjYgMTMgMTAgYyA0LjYzIDIuMzkgMTAuNTQgMy43MiAxNiA1IGMgNS45NCAxLjQgMTEuOTYgMi42OCAxOCAzIGMgMTIuOTEgMC42OCAyNS43IDAuNzMgMzkgMCBjIDM2LjI2IC0yIDczLjAzIC0zLjk1IDEwNiAtOCBjIDUuNDIgLTAuNjcgMTAuNSAtNS4xMiAxNiAtNyBjIDYuMTggLTIuMTIgMTMuMyAtMy4wMiAxOSAtNSBjIDEuNDUgLTAuNSAyLjc2IC0xLjg4IDQgLTMgYyAyLjQ1IC0yLjIzIDUuNjggLTQuNTEgNyAtNyBjIDEuMzYgLTIuNTYgMS43MiAtNi42NyAyIC0xMCBjIDAuMzggLTQuNTggMC44NyAtOS45OCAwIC0xNCBjIC0wLjY0IC0yLjk1IC0yLjk4IC02LjM1IC01IC05IGMgLTMuMTkgLTQuMTkgLTcuMjQgLTkuMzcgLTExIC0xMiBjIC0yLjEzIC0xLjQ5IC02LjMgLTEuMSAtOSAtMiBjIC0xLjA2IC0wLjM1IC0xLjk1IC0xLjc1IC0zIC0yIGMgLTQuMTMgLTAuOTcgLTkuMzIgLTEuOTEgLTE0IC0yIGMgLTExLjkyIC0wLjI0IC0yNC4xMiAtMC4wNiAtMzYgMSBjIC02LjczIDAuNiAtMTQuMiAxLjUgLTIwIDQgYyAtOC4wNSAzLjQ4IC0xNS44IDEwLjI4IC0yNCAxNSBjIC0yLjg1IDEuNjQgLTYuMzIgMi4yNSAtOSA0IGMgLTYuNzkgNC40NSAtMTMuMDcgMTAuMTUgLTIwIDE1IGwgLTEwIDYiLz48L3N2Zz4=";
                        var byteArray = Convert.FromBase64String(svgContent);
                        using (var stream = new MemoryStream(byteArray))
                        {
                            var svgDocument = SvgDocument.Open<SvgDocument>(stream);
                            var bitmap = svgDocument.Draw();
                            var path = webRootPath + ImagesPath.SpecialityPhotos + "abc" + extension;
                            bitmap.Save(path);
                        }
                        //var byteArray = Encoding.ASCII.GetBytes(svgContent);
                        //var path = webRootPath + ImagesPath.SpecialityPhotos + "abc" + extension;
                        //using (var stream = new MemoryStream(byteArray))
                        //{
                        //    var svgDocument = SvgDocument.Open<SvgDocument>(stream);
                        //    var bitmap = svgDocument.Draw();
                        //    bitmap.Save(path);
                        //}
                        //using (var stream = new MemoryStream(byteArray))
                        //{
                        //    var svgDocument = SvgDocument.Open<SvgDocument>(stream);
                        //    var bitmap = svgDocument.Draw();
                        //    //var path = webRootPath + ImagesPath.SpecialityPhotos + "abc" + extension;
                        //    bitmap.Save(path);
                        //}
                    }
                    else
                    {
                        extension = "." + extension;
                    }

                    if (!Directory.Exists(webRootPath + ImagesPath.SpecialityPhotos))
                    {
                        Directory.CreateDirectory(webRootPath + ImagesPath.SpecialityPhotos);
                    }
                    if (!Directory.Exists(webRootPath + ImagesPath.SpecialityThumbPhotos))
                    {
                        Directory.CreateDirectory(webRootPath + ImagesPath.SpecialityThumbPhotos);
                    }

                    string picName = Guid.NewGuid().ToString();

                    List<ImageModel> obj = new List<ImageModel>();

                    ImageModel img = new ImageModel();

                    img.Base64 = base64Data;
                    img.ImageUrl = webRootPath + ImagesPath.SpecialityPhotos + picName + extension;
                    img.ThumbImageUrl = webRootPath + ImagesPath.SpecialityThumbPhotos + picName + extension;
                    obj.Add(img);

                    CommonMethods.SaveImageAndThumb(obj);

                    entity.PhotoPath = picName + extension;
                    entity.PhotoThumbnailPath = picName + extension;

                }
                else if (string.IsNullOrEmpty(entity.PhotoPath) && string.IsNullOrEmpty(entity.PhotoThumbnailPath))
                {
                    entity.PhotoPath = string.Empty;
                    entity.PhotoThumbnailPath = string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return entity;
        }

        /// <summary>
        /// Save patient insurance detail images
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commonMethods"></param>
        /// at




        public PatientInsuranceDetails ConvertBase64ToImageForInsurance(PatientInsuranceDetails entity)
        {
            try
            {
                if (!string.IsNullOrEmpty(entity.Base64Front))
                {
                    List<ImageModel> obj = new List<ImageModel>();

                    string webRootPath = "";

                    //get root path
                    webRootPath = Directory.GetCurrentDirectory();

                    //getting data from base64 url
                    var base64Data = Regex.Match(entity.Base64Front, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;

                    //getting extension of the image
                    string extension = Regex.Match(entity.Base64Front, @"data:image/(?<type>.+?),(?<data>.+)").Groups["type"].Value.Split(';')[0];

                    extension = "." + extension;

                    if (!Directory.Exists(webRootPath + ImagesPath.PatientInsuranceFront))
                    {
                        Directory.CreateDirectory(webRootPath + ImagesPath.PatientInsuranceFront);
                    }
                    if (!Directory.Exists(webRootPath + ImagesPath.PatientInsuranceFrontThumb))
                    {
                        Directory.CreateDirectory(webRootPath + ImagesPath.PatientInsuranceFrontThumb);
                    }

                    string picName = Guid.NewGuid().ToString();

                    

                    ImageModel img = new ImageModel();

                    img.Base64 = base64Data;
                    img.ImageUrl = webRootPath + ImagesPath.PatientInsuranceFront + picName + extension;                    
                    img.ThumbImageUrl = webRootPath + ImagesPath.PatientInsuranceFrontThumb + "thumb_"+picName + extension;
                    obj.Add(img);

                    CommonMethods.SaveImageAndThumb(obj);

                    entity.InsurancePhotoPathFront = picName + extension;
                    entity.InsurancePhotoPathThumbFront = "thumb_" + picName + extension;

                }
                else if (string.IsNullOrEmpty(entity.InsurancePhotoPathFront) && string.IsNullOrEmpty(entity.InsurancePhotoPathThumbFront))
                {
                    entity.InsurancePhotoPathFront = string.Empty;
                    entity.InsurancePhotoPathThumbFront = string.Empty;
                }

                if (!string.IsNullOrEmpty(entity.Base64Back))
                {
                    List<ImageModel> obj = new List<ImageModel>();

                    string webRootPath = "";

                    //get root path
                    webRootPath = Directory.GetCurrentDirectory();

                    //getting data from base64 url
                    var base64Data = Regex.Match(entity.Base64Back, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;

                    //getting extension of the image
                    string extension = Regex.Match(entity.Base64Back, @"data:image/(?<type>.+?),(?<data>.+)").Groups["type"].Value.Split(';')[0];

                    extension = "." + extension;

                    if (!Directory.Exists(webRootPath + ImagesPath.PatientInsuranceBack))
                    {
                        Directory.CreateDirectory(webRootPath + ImagesPath.PatientInsuranceBack);
                    }
                    if (!Directory.Exists(webRootPath + ImagesPath.PatientInsuranceBackThumb))
                    {
                        Directory.CreateDirectory(webRootPath + ImagesPath.PatientInsuranceBackThumb);
                    }

                    string picName = Guid.NewGuid().ToString();

                    ImageModel img = new ImageModel();

                    img.Base64 = base64Data;
                    img.ImageUrl = webRootPath + ImagesPath.PatientInsuranceBack + picName + extension;
                    img.ThumbImageUrl = webRootPath + ImagesPath.PatientInsuranceBackThumb + "thumb_" + picName + extension;
                    obj.Add(img);

                    CommonMethods.SaveImageAndThumb(obj);

                    entity.InsurancePhotoPathBack = picName + extension;
                    entity.InsurancePhotoPathThumbBack = "thumb_" + picName + extension;

                }
                else if (string.IsNullOrEmpty(entity.InsurancePhotoPathBack) && string.IsNullOrEmpty(entity.InsurancePhotoPathThumbBack))
                {
                    entity.InsurancePhotoPathBack     = string.Empty;
                    entity.InsurancePhotoPathThumbBack = string.Empty;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return entity;
        }
    }
}
