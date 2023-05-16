import random as rd
import json
from .ConsolePrint1 import ConsolePrint

import utility.Simulation as sim
import utility.galaxy_generation as gg


#breaks shit????
#galaxy = []
#moving_fleets = []





def start():

    #==== L O A D   O R   C R E A T E   S A V E ============
    try:
        f = open("save.json")
        f2 = open("player-save.json")
        alreadyASave = f.read() != ""
    except FileNotFoundError:
        ConsolePrint("save file not detected, please create an empty file called save.json and another called player-save.json", "s")
        quit()
        f.close()

    if alreadyASave:
        f = open("save.json", "r")
        ConsolePrint("loading previous save", "s")
        data = f.read()
        save = json.loads(data)

        f2 = open("player-save.json", "r")
        player_list = json.loads(f2.read())

        galaxy = save[0]
        moving_fleets = save[1]
        f.close()
        ConsolePrint("save loaded", "s")

    else:
        ConsolePrint("first time running detected, generating save file...", "s")
        f = open("save.json", "w")
        galaxy = gg.generate_galaxy()
        ConsolePrint("galaxy generated", "s")
        moving_fleets = []
        json.dump([galaxy,moving_fleets], f)
        ConsolePrint("save file generated, galaxy saved, moving fleets list created", "s")
        f.close()
    return galaxy, moving_fleets, player_list



def OnPlayerAdd(galaxy, name, system=(-1)): #gives a random non pirate system if no system number  is precised returns the new galaxy


    if system == (-1):
        n = rd.randint(0,len(galaxy)-1)
        while galaxy[n][1] != "none":
            n = rd.randint(0,len(galaxy))
        galaxy[n][1] = name
        ConsolePrint("assigned system "+ str(n)+ " to "+ str(name), "s")
    else:
        try: #raise and prints error if specified system already owned
            if galaxy[system][1] != "none":
                raise ValueError("System already owned, please retry with a valid system number")
            else:
                galaxy[system][1] = name
                ConsolePrint("assigned system "+str(system)+ " to "+ str(name), "s")
        except ValueError as err:
            print(err)
            pass
    return galaxy



def CreateMovingFleet(origin, destination, power, galaxy, moving_fleets, addr): #takes  destination and origin star number and power, adds fleet to moving fleet, substract power from system raise error if not enough fleet power in the system
    #origin int,  destination str, power str
    mf = moving_fleets
    #create fleet
    fleet = [power, destination, galaxy[origin][5], galaxy[origin][1]]
    #substract from system fleet power
    system_fleet_power = galaxy[origin][3]
    try:
        if power > int(system_fleet_power):
            raise ValueError("fleet power is superior to system fleet power, fleet cancelled")
        else:
            galaxy[origin][3] = str(int(galaxy[origin][3])-int(power))
            mf.append(fleet)
            ConsolePrint(addr[0] +":created fleet of "+ str(power)+ " power from "+ str(origin)+ " to "+ str(destination), "s")
    except ValueError as err:
        ConsolePrint(err,"s")
        pass
    return galaxy, mf


def SaveGame(galaxy, moving_fleets, player_list):
    f = open("save.json", "w")
    json.dump([galaxy,moving_fleets], f)
    f.close()
    f = open("player-save.json", "w")
    json.dump(player_list, f)

    ConsolePrint("saved current game!", "s")





"""#for test purposes
galaxy, moving_fleets = start()
OnPlayerAdd("leo",0)
OnPlayerAdd("piglet", 2)
galaxy[0][3] = 51
galaxy[2][3] = 80

for i in galaxy:
    print(i)

CreateMovingFleet(0, "2", 50)

while len(moving_fleets) != 0:
    galaxy, moving_fleets = sim.UpdateSimulation(galaxy, moving_fleets, 0.1)


for i in galaxy:
    print(i)
SaveGame(galaxy, moving_fleets)
"""





























#
