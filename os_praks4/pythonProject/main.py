from datetime import datetime
import time
import ctypes
a = ctypes.c_uint(0)
b = 2
c = 3
i = 0
start_time = datetime.now()

while i < 100000000:
    a.value += b * 2 + c - i
    i += 1
print(a)
print(datetime.now() - start_time)
