# https://leetcode.com/problems/multiply-strings/
class Solution:
    def multiply(self, num1: str, num2: str) -> str:
        num1int=0
        num2int=0
        for i in range(0, len(num1)):
            num1int+=int(num1[i])*(10**(len(num1)-1-i))
        for i in range(0, len(num2)):
            num2int+=int(num2[i])*(10**(len(num2)-1-i))
            
        return str(num1int*num2int)
