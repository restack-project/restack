from pyrestack import restack_client

URL = "http://192.168.5.12:5000/api"
# USERNAME = ""
# PASSWORD = ""
# VERIFY_SSL = True

client = restack_client.ReStackClient(URL, backend='requests')
# print(client.get_all())
print(client.execute(7))