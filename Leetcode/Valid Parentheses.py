# https://leetcode.com/problems/valid-parentheses/
class Solution:
    def isValid(self, s: str) -> bool:
        opened=''
        for i in s:
            if(i in '({['):
                opened+=i
            else:
                if(opened==''):
                    return False
                if((i==')' and opened[-1]=='(') or(i=='}' and opened[-1]=='{') or (i==']' and opened[-1]=='[') ):
                    opened=opened[:len(opened)-1]
                else:
                    return False
        if(opened!=''):
            return False
        return True
