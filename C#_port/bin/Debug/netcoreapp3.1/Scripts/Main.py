import clr
clr.AddReference('raylib_cs')
from Raylib_cs import *
from System import Console
from datetime import datetime
import time
#print(input(":>"))

date = datetime.now()

if 4000000000000 > 400000000000:
	#Console.WriteLine("Massive number!")
	Raylib.DrawText(str(date), 120,120,40, Color.BLUE)
	#Console.Beep()
elif 1 == 400000000000:
	#Console.WriteLine("SOO LARGE!")
	Raylib.DrawText(str(date), 120,120,40, Color.BLUE)
else:
	#Console.WriteLine("Tiny Number...!")
	Raylib.DrawText(str(date), 120,120,40, Color.PINK)



