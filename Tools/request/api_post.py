import requests
import datetime

url = 'http://localhost:39011/api/CloudData?_interface=SevenTinyTest.UserInformation.UndeletedList'

i = 18

data = {
    'Name': '张三_'+str(i),
    'Age': 30,
}

response = requests.post(url, json=data)

print(response.text)
