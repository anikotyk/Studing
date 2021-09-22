# https://leetcode.com/problems/longest-palindromic-substring/
class Solution:
    def longestPalindrome(self, s: str) -> str:
        maxl=0
        res=s[0]
        
        
        for i in range(0, len(s)):
            for j in range(i+1, len(s)+1):
                subs=s[i:j]
                if(len(subs)>maxl and subs==subs[::-1]):
                    res=subs
                    maxl=len(res)
            if(maxl>len(s)-i):
                break
        return res
