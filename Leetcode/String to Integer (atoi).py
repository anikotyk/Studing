# https://leetcode.com/problems/string-to-integer-atoi/
class Solution:
    def myAtoi(self, s: str) -> int:
        res=''
        nums='0123456789'
        for i in range(0,len(s)):
            if (s[i]==' '):
                if(res!=''):
                    break
                continue
            elif(s[i]=='+' or s[i]=='-'):
                if (i<len(s)-1):
                    if(s[i+1] not in nums):
                        break
                else:
                    break
                if (i>0):
                    if(s[i-1] in nums):
                        break

            elif(s[i] not in nums):
                break
            res+=s[i]
        if(res==''):
            return 0
        if(int(res)>2**31-1):
            return 2**31-1
        if(int(res)<-2**31):
            return -2**31
        return int(res)
