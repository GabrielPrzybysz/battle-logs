from http.server import HTTPServer, BaseHTTPRequestHandler
import socketserver
import json


PORT = 1234


class myHandler(BaseHTTPRequestHandler):


    def do_POST(self):
        self.send_response(200)
        self.send_header('Content-type','text/html')
        self.end_headers()

        content_len = int(self.headers.get('Content-Length'))
        decoded__post_body = self.rfile.read(content_len).decode('utf-8')
        loadedJson = json.loads(decoded__post_body)

        logs = loadedJson['log']
        session = loadedJson['session']

        with open(f'./{session}.txt', 'a') as myfile:
            myfile.write(logs + "\n")

        self.wfile.write('200'.encode('utf-8'))
        return



with socketserver.TCPServer(('', PORT), myHandler) as httpd:
    print('Serving at port: ', PORT)
    httpd.serve_forever()