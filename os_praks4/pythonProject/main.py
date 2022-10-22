from datetime import datetime
import time

a = 0
b = 3
c = 3
i = 0
start_time = datetime.now()

while i <= 100000000:
    a += b * 2 + c - i
    i += 1

print(datetime.now() - start_time)