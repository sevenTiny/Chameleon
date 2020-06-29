import requests
import datetime

url = 'http://localhost:39011/api/BatchCloudData?_interface=SevenTinyTest.UserInformation.AddUserInformation'

datas = []
for i in range(1, 100):
    data = {
        'Name': '张三_'+str(i),
        'Age': i
    }

    datas.append(data)

response = requests.post(url, json=datas)

print(response.text)
