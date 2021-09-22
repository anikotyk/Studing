# https://leetcode.com/problems/reverse-integer/
class Solution:
    def reverse(self, x: int) -> int:
        res = str(x)[::-1]
        if (x<0):
            res='-'+res[:len(res)-1]
        res =int(res)
        if (res>=2**31 or res<-2**31):
            return 0
        return res
