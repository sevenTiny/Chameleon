import requests
import datetime

url = 'http://prod.7tiny.com:39011/api/BatchCloudData?_interface=ChameleonDemo.UserInformation.BatchAddUserInformation'

datas = []
for i in range(1, 1000):
    data = {
        'BookName': '儿童生活中的十万个为什么'+str(i),
        'Admin': 'zhangsan',
        'Type': 1,
        'ISBN': "ISBN-"+str(i),
        'BuyTime': str(datetime.datetime.now()),
        'Publisher': '清华大学出版社',
    }

    datas.append(data)

headers = {
    "Cookie": "_AccessToken=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiIxNTk0MDE4NjgxMjU3IiwiVXNlckVtYWlsIjoic2V2ZW50aW55QGZveG1haWwuY29tIiwiVXNlck5hbWUiOiJzZXZlbnRpbnkiLCJDaGFtZWxlb25Sb2xlIjoiMiIsIk9yZ2FuaXphdGlvbiI6IjA5YjA4NjJiLTVhNWMtNGE1ZS05ZThkLWI3ODJiNDUyNDZiYSIsImV4cCI6MTU5NDEwNzEyNCwiaXNzIjoicHJvZC43dGlueS5jb20iLCJhdWQiOiJwcm9kLjd0aW55LmNvbTozOTAzMSJ9.gU4BJGcbdr1Ul9d7BHRe04-LyVna_Vwk8Pu9XDlJaFM;",
    "content-type": "charset=utf8"
}

response = requests.post(url, json=datas, headers=headers)

print(response.text)
