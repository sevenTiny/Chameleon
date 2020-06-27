import requests
import datetime

url = 'http://localhost:39011/api/CloudData?_interface=SevenTinyTest.UserInformation.UpdateNameByAge&age=99'

data = {
    'Name': '虎虎虎'
}

response = requests.put(url, json=data)

print(response.text)
