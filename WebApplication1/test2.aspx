﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test2.aspx.cs" Inherits="WebApplication1.test2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>

    
<%--    <style>
.div1 {
    position:absolute;
    width:10%;
  margin-top:10%;
  margin-left:10%;
  z-index:99;
  text-align: center;
}
.div2 {
    position:absolute;
    width:10%;
  margin-top:20%;
  margin-left:15%;
  z-index:99;
  text-align: center;
}
.div3 {
    position:absolute;
    width:10%;
  margin-top:100px;
  margin-left:200px;
  z-index:99;
  text-align: center;
}
</style>--%>
</head>
<body <%--style="background-image: url('map.png'); background-repeat:no-repeat; background-size:cover; width:100%; "--%>>
    <form id="form1" runat="server">
 

        <div class="col2" data-bind="dxCircularGauge: gaugeRequestsNumberOptions"></div>
        <style>
            body
{
    position: relative;
    background-color: gray;
    margin: 0;
}

.layout
{
    width: 1270px;
    font-family: 'Segoe UI', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif;
    font-weight: 400;
    margin: 0 auto;
}

.dark
{
    background-color: #363e5b;
}

.light
{
    background-color: white;
}

.header-text
{
    display: inline-block;
    font-size: 30px;
    font-family: 'Segoe UI Light', 'Helvetica Neue Light', 'Segoe UI', 'Helvetica Neue', Helvetica, 'Trebuchet MS', 'Droid Sans', Tahoma, Geneva, sans-serif;
    font-weight: 200;
    margin-left: 100px;
    padding: 60px 0 0 0;
}

.dark .header-text
{
    color: white;
}

.light .header-text
{
    color: #43474b;
}

.img-theme
{
    float: right;
    width: 60px;
    height: 50px;
    margin-right: 100px;
    margin-top: 60px;
    z-index: 101;
    background-image: url(sprite.png);
    background-position: -90px 0;
    cursor: pointer;
}

.light .img-theme
{
    background-position: 0 0;
}

.content
{
    width: 1070px;
    height: 750px;
    padding: 40px 100px;
}

.slide-button
{
    position: absolute;
    top: 430px;
    background-image: url(sprite.png);
    width: 40px;
    height: 80px;
    cursor: pointer;
}

.slide-button.right-button
{
    right: 0;
    background-position: -90px -55px;
}

.light .slide-button.right-button
{
    background-position: 0px -55px;
}

.slide-button.left-button
{
    left: 0;
    background-position: -135px -55px;
}

.light .slide-button.left-button
{
    background-position: -45px -55px;
}

.row
{
    width: 1070px;
    height: 180px;
    margin-top: 10px;
    overflow: hidden;
}

.traffic .row
{
    height: 340px;
}

.legend-row
{
    position: relative;
    height: 17px;
    width: 1500px;
}

.row::after .legend-row::after
{
    content: ' ';
    display: block;
    height: 0;
    clear: both;
    visibility: hidden;
}

.legend
{
    font-family: 'Segoe UI', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif;
    font-weight: 400;
    font-size: 15px;
    margin-right: 3px;
    z-index: 10;
    float: left;
    color: white;
}

.light .legend
{
    color: #43474b;
}

.hr
{
    width: 1100px;
    height: 10px;
    border-bottom: 1px solid #6b7289;
    float: left;
}

.light .hr
{
    border-bottom: 1px solid #c0c0c0;
}

.clear
{
    clear: both;
}

.helth .row .col1
{
    width: 210px;
    height: 170px;
    float: left;
}

.traffic .row .col1
{
    float: left;
    width: 420px;
    height: 320px;
    overflow: hidden;
}

.helth .row .col2
{
    width: 210px;
    height: 175px;
    float: left;
    margin: -10px 0 0 0;
}

.traffic .row .col2
{
    width: 600px;
    height: 320px;
    float: right;
    overflow: hidden;
}

.helth .row .col3
{
    height: 165px;
    float: left;
    width: 650px;
}

.col1 .text
{
    -ms-word-wrap: normal;
    word-wrap: normal;
    font-size: 11px;
    color: #8e93a7;
    width: 200px;
    margin-top: 20px;
}

.col1 .label-value
{
    font-family: 'Segoe UI Semibold', 'Helvetica Neue Medium', 'Segoe UI', 'Helvetica Neue', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif;
    font-size: 60px;
    line-height: 60px;
    font-weight: 600;
}

label#requestNumber
{
    color: #7cd2c7;
}

.light label#requestsNumber
{
    color: #76c8bd;
}

label#CPU
{
    color: #75c0e0;
}

label#memoryConsumption
{
    color: #7e8ab6;
}

.light label#memoryConsumption
{
    color: #96a3d4;
}

.traffic .chart-container
{
    width: inherit;
    height: 300px;
}

.traffic .chartContainer
{
    width: inherit;
    height: 300px;
}


        </style>


    </form>
</body>
</html>
