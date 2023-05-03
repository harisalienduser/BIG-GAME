using AnotherFileBrowser.Windows;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using UnityGoogleDrive;
public class FileBrowserUpdate : MonoBehaviour
{
    public RawImage rawImage;
    public string UploadFilePath;

    private GoogleDriveFiles.CreateRequest request;
    private string result;
    public void OpenFileBrowser()
    {
        var bp = new BrowserProperties();
        bp.filter = "Image files (*.ppt, *.pptx) | *.ppt; *.pptx";
        bp.filterIndex = 0;

        new FileBrowser().OpenFileBrowser(bp, path =>
        {
            //Load image from local path with UWR
           Upload(false,path);
        });
    }

  
    IEnumerator UploadFile(string filePath, string url)
    {
        byte[] fileData = File.ReadAllBytes(filePath);
        WWWForm form = new WWWForm();
        form.AddField("fieldName", "fieldValue"); // add any additional form fields
        form.AddBinaryData("file", fileData, "fileName.pptx", "application/vnd.ms-powerpoint"); // add the file data
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("File upload complete!");
        }
    }
    private void Upload(bool toAppData, string url)
    {
        UploadFilePath = url;
        var content = File.ReadAllBytes(UploadFilePath);
        if (content == null) return;

        var file = new UnityGoogleDrive.Data.File() { Name = Path.GetFileName(UploadFilePath), Content = content };
        if (toAppData) file.Parents = new List<string> { "appDataFolder" };
        request = GoogleDriveFiles.Create(file);
        request.Fields = new List<string> { "id", "name", "size", "createdTime" };
        request.Send().OnDone += PrintResult;
    }

    private void PrintResult(UnityGoogleDrive.Data.File file)
    {
        result = string.Format("Name: {0} Size: {1:0.00}MB Created: {2:dd.MM.yyyy HH:MM:ss}\nID: {3}",
                file.Name,
                file.Size * .000001f,
                file.CreatedTime,
                file.Id);
    }

}
