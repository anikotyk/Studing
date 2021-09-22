# https://leetcode.com/problems/integer-to-roman/
class Solution:
    def intToRoman(self, num: int) -> str:
        res=''
        if (num>=1000):
            res+='M'*int(num/1000)
            num=num%1000
        if (num>=900):
            res+='CM'
            num=num%900
        if (num>=500):
            res+='D'
            num=num%500
        if (num>=400):
            res+='CD'
            num=num%400
            
        if (num>=100):
            res+='C'*int(num/100)
            num=num%100
        if (num>=90):
            res+='XC'
            num=num%90
        if (num>=50):
            res+='L'
            num=num%50
        if (num>=40):
            res+='XL'
            num=num%40
            
        if (num>=10):
            res+='X'*int(num/10)
            num=num%10
        if (num>=9):
            res+='IX'
            num=num%9
        if (num>=5):
            res+='V'
            num=num%5
        if (num>=4):
            res+='IV'
            num=num%4
        if (num>=1):
            res+='I'*num
        return res
