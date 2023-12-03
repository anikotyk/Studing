module Main where

import Gauss (Row, Matrix, Vector, valueToMul, force, subRows, multiplyRowNumber, maxIndex, infinity, goAlongRows, findMainPivot, gaussAlgo, concatMatrixVector, checkResult, generateMatrix, generateRandom, force, CustomAns (Exists))
import Control.Concurrent
import GHC.Conc ( numCapabilities, pseq )
import System.Environment (getArgs)
import GHC.Conc.Sync (par)
import Text.Printf (printf)
import System.Random (StdGen, getStdGen, randoms)
import Data.Time.Clock (diffUTCTime, getCurrentTime)


main :: IO ()
main = do
  let size = 300    
  putStrLn "Par"
  (mat1, vec1) <- generateRandom size
  start <- getCurrentTime
  let ans = gaussAlgo mat1 vec1 True
  let x = case ans of
        Exists vec -> vec
        _ -> []
  let y = force x
  print y
  end <- getCurrentTime
  putStrLn $ show (end `diffUTCTime` start) ++ " elapsed"
  putStrLn $ "number cores: " ++ show numCapabilities

  putStrLn "Seq"
  (mat1, vec1) <- generateRandom size
  start <- getCurrentTime
  let ans = gaussAlgo mat1 vec1 False
  let x = case ans of
        Exists vec -> vec
        _ -> []
  let y = force x
  print y
  end <- getCurrentTime
  putStrLn $ show (end `diffUTCTime` start) ++ " elapsed"
  putStrLn "number cores: 1"