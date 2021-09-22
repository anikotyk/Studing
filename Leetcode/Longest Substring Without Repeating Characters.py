# https://leetcode.com/problems/longest-substring-without-repeating-characters/
class Solution:
    def lengthOfLongestSubstring(self, s: str) -> int:
        res=0

        for i in range(0, len(s)):
            s1=s[i:]
            hp=""
            num=0
            for j in s1:
                if j not in hp:
                    hp+=j
                    num+=1
                    if num>res:
                        res=num
                else:
                    break

        return res

