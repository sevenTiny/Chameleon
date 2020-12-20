import requests
import datetime

url = 'http://dev.7tiny.com:39011/api/BatchCloudData?_interface=ChameleonDemo.UserInformation.BatchAddUserInformation'

datas = []
age = 1
for i in range(1, 100):

    if(age >= 100):
        age = 1

    data = {
        'Name': 'User'+str(i),
        'Age': age,
        'JoinTime': str(datetime.datetime.now()),
        'Sex': i % 2
    }

    age = age+1

    datas.append(data)

headers = {
    "Cookie": "_AccessToken=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiIxNTk0MDE4NjgxMjU3IiwiVXNlckVtYWlsIjoic2V2ZW50aW55QGZveG1haWwuY29tIiwiVXNlck5hbWUiOiJzZXZlbnRpbnkiLCJDaGFtZWxlb25Sb2xlIjoiMiIsIk9yZ2FuaXphdGlvbiI6IjA5YjA4NjJiLTVhNWMtNGE1ZS05ZThkLWI3ODJiNDUyNDZiYSIsImV4cCI6MTU5NDI5NTYzOSwiaXNzIjoiZGV2Ljd0aW55LmNvbSIsImF1ZCI6ImRldi43dGlueS5jb206MzkwMzEifQ.wOOXRCuw08n4YfRxurSGA9-p7dU4p9doiUTczB2nALw;"
}

response = requests.post(url, json=datas, headers=headers)

print(response.text)
