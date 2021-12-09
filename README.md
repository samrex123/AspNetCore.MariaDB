# AspNetCore.MariaDB

<b>Why did you build this project?</b><br>
This c# API is a part of the whole application for a decentralized social media platform.

<b>Frontend <---> this api <---> user database</b><br>

Frontend pings this api to read from their MariaDB or send out mail to all the users with the information that is created, changed or deleted.<br>

(Another application is used to read the email and insert it in the users database)<br>


<b>Technologies Used</b><br>
Mailkit - .Net 5 - EnitiyFrameworks - Newton.Json - Pomelo.MySql


<b>What was your motivation?</b><br>
Our motivation for building this part comes from the customers request to create a forum and not have information stored in the cloud by the big corperations.



<b>What problem does it solve?</b><br>
 It was made to communicate between nodes in the network, so that all information stays local.

<b>What did you learn?</b><br>
Communication between frontend and backend.<br>
How to use a mailservice.<br>
How to create a standard CRUD API.<br>
How to send information from the Database (MariaDB)<br>
How to encrypt text.<br>


<b>FEATURES</b><br>
Using mail for communication between users.

<b>DOCUMENTATION</b>

<b>Database structure</b><br>
![image](https://user-images.githubusercontent.com/48559023/145374531-1f49bc2b-eb95-4530-9ef3-dd22bd2af57c.png)<br>


Our forum is based on a Discussion object that has posts and the posts have comments. All show which user is writing.
The users in our DB is the ones whom will sync their database with the mailservice.

<b>API</b><br>
The API is for CRUD operations in the database.
Each model-item has its own controller.<br>

<b>HttpGet("/api/[controller]")</b><br>
![image](https://user-images.githubusercontent.com/48559023/145376247-28e9c2b0-15fc-4a31-8385-08ff0748d1d3.png)<br>
<b>HttpPut</b><br>
![image](https://user-images.githubusercontent.com/48559023/145376637-6332b333-92bc-405e-8b1a-679a94b6cd0f.png)<br>
<b>HttpPost</b><br>
![image](https://user-images.githubusercontent.com/48559023/145376925-a4d079ac-5565-45fd-ab2f-5697e796c52c.png)<br>
<b>HttpDelete</b><br>
![image](https://user-images.githubusercontent.com/48559023/145377257-8cb8acf7-e3dd-434f-9edd-4d5c0fa2f4cf.png)<br>

<b>Method used in controller turns what happend into an SQL script</b><br>
![image](https://user-images.githubusercontent.com/48559023/145377508-6908fdfe-a756-40d6-a779-5c11da8ff742.png)<br>
<b>Mailmethod</b><br>
![image](https://user-images.githubusercontent.com/48559023/145378043-acae6f2f-9b92-4dea-89ea-b2dd37a9a87d.png)<br>

<b>Encryption</b><br>
![image](https://user-images.githubusercontent.com/48559023/145378198-ccb89b3b-ecbc-43fb-b97a-bb4faa1128fc.png)<br>

  
 <b>Final thoughts</b><br>
This application was built in the first sprint. It still requires a lot of polishing to make it a viable product. But its a foundation. <br>

  <b>To be continued...</b>
