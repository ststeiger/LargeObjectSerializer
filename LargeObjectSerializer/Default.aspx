<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LargeObjectSerializer._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style type="text/css">

        html, body
        {
            width: 100%;
            height: 100%;
            margin: 0px;
            padding: 0px;
            overflow: hidden;
        }

    </style>

</head>
<body>
    <form id="form1" runat="server">
    </form>

    
    <div style="width: calc(100% - 0.75cm); height: 100%; margin: 0.25cm; display: block;">
        <a href="ajax/serialize.ashx" target="_blank">Test</a>
        
        <h4 style="margin-top: 0.25cm;" id="hStatus">AJAX request started - Serialization in progress</h4>
        <textarea id="myTxt" style="width: 100%; height: 80%;" rows="10" cols="10">

        </textarea>
    </div>

    <script type="text/javascript">
        function loadXMLDoc()
        {
            var xmlhttp;

            if (window.XMLHttpRequest)
            {
                // code for IE7+, Firefox, Chrome, Opera, Safari
                xmlhttp = new XMLHttpRequest();
            } else
            {
                // code for IE6, IE5
                xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
            }

            xmlhttp.onreadystatechange = function ()
            {
                // console.log(xmlhttp.readyState, ": ", xmlhttp.status)
                // console.log(xmlhttp.responseText);

                
                if (xmlhttp.readyState == XMLHttpRequest.DONE)
                {
                    var hStatus = document.getElementById("hStatus");

                    if (xmlhttp.status == 200)
                    {
                        hStatus.textContent = "Serialization finished successfully.";


                        // var obj = document.querySelector("#myDiv");
                        var myTxt = document.getElementById("myTxt");
                        
                        if (myTxt)
                            myTxt.value = xmlhttp.responseText;
                    }
                    else if (xmlhttp.status == 400)
                    {
                        hStatus.textContent = "There was an error 400.";
                        console.log('There was an error 400')
                    }
                    else if (xmlhttp.status == 0)
                    {
                        hStatus.textContent = "AJAX-request was aborted.";
                        console.log('AJAX-request was aborted.')
                    }
                    else
                    {
                        hStatus.textContent = "Something else other than status 200 was returned.";
                        console.log('something else other than 200 was returned')
                    }
                }
            };

            xmlhttp.open("GET", "ajax/serialize.ashx", true);
            xmlhttp.send();
        }

        loadXMLDoc();

</script>



</body>
</html>
