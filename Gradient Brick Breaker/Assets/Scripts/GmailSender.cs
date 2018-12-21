using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class GmailSender : MonoBehaviour
{
    private MailMessage mail;
    SmtpClient smtpServer;
    string systemInfo;

   public void Init()
    {
        mail = new MailMessage();

        mail.From = new MailAddress("gradientbrickbreaker@gmail.com");
        mail.To.Add("gradientbrickbreaker@gmail.com");
        mail.Subject = "Gradient Brick Breaker feedback to version " + Application.version;
       

        smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential("gradientbrickbreaker@gmail.com", "hpVgoUHeCa5TVou9m0Wr") as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };

        systemInfo = "Device type: " + SystemInfo.deviceType + "\n"
        + "Device model: " + SystemInfo.deviceModel;


    }

    public void SendMail()
    {
        string feedback = GameManager.instance.GetUIManager().feedback_nolike_inputfield.GetComponent<InputField>().textComponent.text.ToString();
        if (feedback != "")
        {
            mail.Body = systemInfo + "\n\nFeedback:\n" + feedback.ToString();
            smtpServer.Send(mail);
            Debug.Log("success");
        }
    }

    public void GoToPlayMarket()
    {
        Application.OpenURL("market://details?id=ru.devalkone.gbbreaker");
    }
}