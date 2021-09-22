# https://leetcode.com/problems/add-digits/
class Solution:
    def addDigits(self, num: int) -> int:
        stringsum=str(num)
        while(len(stringsum)!=1):
            suma=0
            for i in stringsum:
                suma+=int(i)
            stringsum=str(suma)
        return int(stringsum)
