using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Galleri.Model;

namespace Galleri
{
    public partial class Default : System.Web.UI.Page
    {
        private Gallery _gallery;

        private Gallery Gallery
        {
            get { return _gallery ?? (_gallery = new Gallery()); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["img"] != null)
            {
                Image2.ImageUrl = "~/pics/" + Request.QueryString["img"];
                
            }
            if (Request.QueryString["Success"] == "true")
            {
                SuccessLabel.Visible = true;
            }
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                if (FileUpload.HasFile)
                {
                    try
                    {
                        var name = Gallery.SaveImage(FileUpload.FileContent, FileUpload.FileName);
                        Response.Redirect(String.Format("?img={0}{1}", name, "&Success=true"));
                    }
                    catch
                    {
                        CustomValidator message = new CustomValidator();
                        message.IsValid = false;
                        message.ErrorMessage = "Något gick fel vid uppladdningen av din fil :<";
                        this.Page.Validators.Add(message);
                        SuccessLabel.Visible = false;
                    }
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }

        public IEnumerable<Galleri.Model.Images> ThumbRepeater_GetData()
        {
            return Gallery.GetImageNames();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            SuccessLabel.Visible = false;
        }

        protected void ThumbRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var images = (Images)e.Item.DataItem;
            if (images.name == Request.QueryString["img"])
            {
                var link = (HyperLink)e.Item.FindControl("HyperLink");
                link.CssClass = "frame";
            }
        }
    }
}