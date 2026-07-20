# Install SSL certificates issued by HM Land Registry as trusted root.
The SSL certificates issued by HM Land Registry need to be installed as trusted root certificates on the machine where the service handler is hosted. The process to install the certificates is as follows:

1.	Copy the root and intermediate certificate files issued by HM Land Registry to the machine where the service handler is hosted.
    1. [The test root certificate can be found here](./certs/HMLR_RootCA.md)
    1. [The test intermediate certificate can be found here](./certs/HMLR_IntermediateCA.md)
    1. [The client certificate file can be found here](./certs/HMLR_ClientCert.md)   
1.	Double click on the root and intermediate certificate files to open the certificate details window.
1.	Click on the Install Certificate button.
1.	The Certificate Import Wizard will be shown. Select Local Machine as the store location.

    ![Install Certificate](.attachments/ServiceHandler/InstallCertificate.png =200x300)

1.	Click Next.
1.	Select Place the certificates in the following store locations by clicking on the Browse button.
    1. place the Root certificate in the Trusted Root Certification Authorities store.
    2. place the Intermediate certificate in the Intermediate Certification Authorities store.
1.	Click Next.
1.	Click Finish.

1. To complete the installation of the client certificate with the private key.
    1. Open a Command Prompt as Administrator.
    1. Run the following command to associate the private key with the certificate. Replace the placeholder values.

        ```powershell
            openssl pkcs12 -export -out full_cert.pfx -inkey privatekey.key -in KnightFrank-BGVendor.cer
            Enter pass phrase for privatekey.key: "<enter password saved in the csr generation process>"
            Enter Export Password: "<enter password saved in the csr generation process>"
            Verifying - Enter Export Password: "<enter password saved in the csr generation process>"
        ```
    1. Import the PFX file using the following command:

        ```powershell
            certutil -f -p "<enter password saved in the csr generation process>" -importpfx full_cert.pfx
        ```

     1. If successful, the output will confirm the private key was found and associated. Refresh your certificate manager; the key icon should now appear on the certificate

1. To Verify, follow these steps:
   1. In CertMgr (Certificate Manager), navigate to the Personal > Certificates folder.
   2. Locate the client certificate you just installed.
   3. Double-click the certificate you just imported.
   4. Check the bottom of the "General" tab. If you do not see a small key icon and the message "You have a private key that corresponds to this certificate," the request is incomplete. 
   
   ![Complete Certificate Installation](.attachments/ServiceHandler/bgtest-complete.png =200x300)



The installed certificates can be viewed using the Microsoft Management Console (MMC) snap-in for certificates. To open the snap-in, run the command `mmc.exe` and add the Certificates snap-in for the Local Computer account.

![Installed Local Certificate](.attachments/ServiceHandler/InstalledLocalCertificates.png)

Once the certificates are installed, you can verify that it was successful by going to the business gateway URL in a web browser. If the certificates are installed correctly, you should not see any SSL/TLS errors when accessing the URL.
https://bgtest.landregistry.gov.uk

* You should see a secure padlock icon in the address bar indicating that the connection is secure.
* You should see a gateway welcome page similar to the one shown below.

![Business Gateway Welcome Page](.attachments/ServiceHandler/BusinessGatewayWelcomePage.png)






