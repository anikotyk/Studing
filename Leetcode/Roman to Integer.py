# https://leetcode.com/problems/roman-to-integer/
class Solution:
    def romanToInt(self, s: str) -> int:
        data={'I':1,'V':5, 'X':10, 'L':50, 'C':100, 'D':500, 'M':1000}
        res=0
        for i in range(0, len(s)):
            if (i<len(s)-1):
                if(s[i]=='I' and s[i+1] in 'VX'):
                    res-=data[s[i]]
                    continue
                elif (s[i]=='X' and s[i+1] in 'LC'):
                    res-=data[s[i]]
                    continue
                elif (s[i]=='C' and s[i+1] in 'DM'):
                    res-=data[s[i]]
                    continue
            res+=data[s[i]]
        return res
