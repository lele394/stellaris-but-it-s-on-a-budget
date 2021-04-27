from _thread import *
import time
import utility.galaxy_generation as gg
import sys
import socket
import utility.t as t
import os
from utility.ConsolePrint1 import ConsolePrint


#CONNECTIVITY
HOST = '127.0.0.1'  # Standard loopback interface address (localhost)
PORT = 65432        # Port to listen on (non-privileged ports are > 1023)

#SAVING
AutoSaveTime = 3600  #3600 is default, do not lower too much, might cause issue with the server

#GAME SIM
UpdateTime = 30  #Update sim of 1 tick every x seconds, keep this one high, lower value may add unnecessary stress on the server (client update requests timed with UpdateTime)
DeltaTime = 0.1  #Basically is "game speed", keep it low or do not touch if you don't know what you're doing, too high might lead to fleets jumping into systems from far away





galaxy = gg.generate_galaxy()
moving_fleets = []
player_list = [["yolo", "#7deb34", "0"]]




def thread_AutoSave(SaveTime=3600):# saves every time seconds / default = 1h
    global galaxy
    global moving_fleets
    global player_list
    ConsolePrint("AutoSave Thread started", "s")
    while True:
        time.sleep(SaveTime)
        g = galaxy
        mf = moving_fleets
        t.SaveGame(g,mf, player_list)
        ConsolePrint("Save Complete", "s")



def Home():
    a = '''
       --- /!\ W I P ---
  ╔════════════════════════╗
  ║ Server Home Screen :D  ║
  ╚════════════════════════╝
  nothing here but it's cool
     type help for help
  ~~~~~~~~~ CONSOLE ~~~~~~~~
    '''
    print(a)
#╦╠║╚╗╔═╝





#client thread
def ClientThread(conn, addr):
    try:
        IsRunning = True


        while IsRunning:
            PlayerName = ""
            global galaxy
            global moving_fleets
            global player_list
            answer = "received"
            data = str(conn.recv(4096))[2:-1]
            ConsolePrint(addr[0] + ":" + data , "c")
            if data =="REQ:GAMEMAPLENGTH":
                answer = str(len(galaxy))
                pass

            elif "PLAYERNAME:" in data:
                PlayerName = data[11:]
                ConsolePrint(addr[0] + ':' +"Now logged in as " + PlayerName,"c")

            elif "REQ:GAMEMAP" in data:
                message = ""
                split_dat = data.split(":")
                first = int(split_dat[2])
                last = int(split_dat[3])

                for i in range(first, last, 1):
                    sub = ""
                    for element in galaxy[i]:
                        sub += str(element) + ":"
                    sub = sub[:-1]
                    message += sub + "/"
                message = message[:-1]
                answer = message

            elif "REQ:PLAYERLIST" in data:
                message = ""
                for player in player_list:
                    sub = ""
                    for element in player:
                        sub += str(element) + ":"
                    sub = sub[:-1]
                    message += sub + "/"
                message = message[:-1]
                answer = str(message)







            if data == "Disconnected, terminating thread":
                IsRunning = False
            else:
                conn.sendall(str.encode(answer))
    except ConnectionAbortedError:
        ConsolePrint(addr[0] + ": Disconnected", "c")







def ServerThread(a):
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.bind((HOST, PORT)) #bind server to the parameters
    s.listen()
    ConsolePrint("server started", "s")
    while True:
        conn, addr = s.accept()
        ConsolePrint('accepted connection from: ' + addr[0] + ':' + str(addr[1]), "s")
        start_new_thread(ClientThread, (conn, addr,))



















#load or create save
ConsolePrint("======== CREATING/LOADING SAVE ========", "s")
galaxy, moving_fleets = t.start() #loads save
ConsolePrint("============ LOADING  DONE ============", "s")
ConsolePrint("Starting server thread...", "s")
start_new_thread(ServerThread, (1,))
ConsolePrint("Server thread started!", "s")
start_new_thread(thread_AutoSave, (AutoSaveTime,))
Home()

while True:
    entry = input("")

    if "os." in entry:
        os.system(entry[3:])
    elif entry == "home":
        Home()


    elif "game." in entry: #executes game mechanics command
        entry = entry[5:]
        if entry == "save":
            print("Saving...")
            t.SaveGame(galaxy, moving_fleets, player_list)
            print("Save complete!")
        if "system" in entry:
            entry = entry[6:]
            if "changeowner" in entry:
                entry = entry[13:]
                Split = entry.split(" ")
                galaxy[int(Split[0])][1] = Split[1]

            if "setfleetpower" in entry:
                entry = entry[15:]
                Split = entry.split(" ")
                galaxy[int(Split[0])][3] = Split[1]

            if "setname"in entry:
                entry = entry[9:]
                Split = entry.split(" ")
                galaxy[int(Split[0])][0] = Split[1]


    elif entry =="help":
        print(open("Help.txt", "rb").read().decode('utf8'))
    elif entry == "quit":
        print("Shutting down server...")
        print("Saving")
        t.SaveGame(galaxy, moving_fleets, player_list)
        print("Save done, killing process")
        quit()









#
