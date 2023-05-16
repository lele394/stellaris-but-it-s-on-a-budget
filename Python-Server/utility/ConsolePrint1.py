import time

def ConsolePrint(string, type):#type "c" client, "s" server
    clock = time.strftime('%X')
    if type == "c":
        print(clock, " [CLIENT]: ", string)
    elif type == "s":
        print(clock, " [SERVER]: ", string)
