# https://leetcode.com/problems/zigzag-conversion/
class Solution:
    def convert(self, s: str, numRows: int) -> str:
        maxdif=(numRows-1)*2
        if(maxdif<2):
            maxdif=1
        
        dif1=maxdif
        dif2=maxdif
        
        res=''
        
        for i in range(0, numRows):
            k=i
            flag=True
            while(k<len(s)):
                res+=s[k]
                if(flag):
                    k+=dif1
                else:
                    k+=dif2
                flag=not flag
            dif1-=2
            dif2+=2
            if(dif1==0):
                dif1=maxdif
                dif2=maxdif
            if(i==0):
                dif2=2
            
        
        
        return res
