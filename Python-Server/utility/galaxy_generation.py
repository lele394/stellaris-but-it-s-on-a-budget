import random as rd
from math import cos,sin,tan, pi
from .ConsolePrint1 import ConsolePrint


seed = 123456


in_circle_radius = 20
out_circle_radius = 80
star_number = 370 #100 for security, too high breaks everything

pirate_chance = 0.12
pirate_strength_station = (0, 0)#is not displayed in client
pirate_strength_fleet = (5, 50)



alphabet = "AZERTYUIOPMLKJHGFDSQWXCVBN"
separator = "-"


def generate_galaxy(seed=rd.random()):
    rd.seed(seed)

    starList = []
    for i in range(star_number):
        star = ["name", "owner", "station", "fleet", "anomaly", ("x position", "y position")]

        #===== N A M E =====
        prefix = ""
        for i in range(3):
            prefix += rd.choice(alphabet)
        star[0] = prefix + separator + str(int(rd.random()*1000000))

        #==== O W N E R =====
        if rd.random()<pirate_chance:
            star[1] = "pirate"
        else:
            star[1] = "none"

        #==== S T A T I O N ====
        if star[1] == "pirate":
            station = [str(int(rd.randrange(pirate_strength_station[0], pirate_strength_station[1] ))), "1", [] ]
        else:
            station = [0,0,[]]
        star[2] = station

        #==== F L E E T ====
        if star[1] == "pirate":
            fleet = str(int(rd.randrange(pirate_strength_fleet[0], pirate_strength_fleet[1] )))
        else:
            fleet = 0
        star[3] = fleet

        #==== A N O M A L Y ====
        star[4] = "none"

        #==== P O S I T I O N ====
        """ #non uniform method
        r = rd.randrange(in_circle_radius, out_circle_radius)
        theta = (rd.randrange(0, 360)/360)   * 2*pi
        position = (r*cos(theta), r*sin(theta))
        """
        #uniform method
        t = 2*pi*rd.random()   # https://stackoverflow.com/questions/5837572/generate-a-random-point-within-a-circle-uniformly
        u = rd.randrange(in_circle_radius, out_circle_radius)+rd.randrange(in_circle_radius, out_circle_radius)
        r = 0
        if u>1: r = 2-u
        else: r= u
        position = (r*cos(t), r*sin(t))


        star[5] = position


        starList.append(star)

    return starList






def test(): #plots galaxy using pyplot
    import matplotlib.pyplot as plt
    g = generate_galaxy(12345)

    x = []
    y = []
    xp = []
    yp = []

    for star in g:
        if star[1] == "none":
            x.append(star[5][0])
            y.append(star[5][1])
        else:
            xp.append(star[5][0])
            yp.append(star[5][1])

    plt.scatter(x,y)
    plt.scatter(xp,yp, color="r")
    plt.show()

#test()






























#
