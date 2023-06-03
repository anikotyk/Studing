from random import randrange
from hashlib import sha1
from gmpy2 import xmpz, to_binary, invert, powmod, is_prime


def generatePQ(bitLength, securityParameter):
    # Generate large prime numbers p and q
    primeBitLength = securityParameter  # primeBitLength >= 160
    primeCount = (bitLength - 1) // primeBitLength
    bitCount = (bitLength - 1) % primeBitLength
    while True:
        # Generate q
        while True:
            seed = xmpz(randrange(1, 2 ** (primeBitLength)))
            a = sha1(to_binary(seed)).hexdigest()
            seedPlusOne = xmpz((seed + 1) % (2 ** primeBitLength))
            z = sha1(to_binary(seedPlusOne)).hexdigest()
            U = int(a, 16) ^ int(z, 16)
            mask = 2 ** (securityParameter - 1) + 1
            q = U | mask
            if is_prime(q, 20):
                break
        # Generate p
        counter = 0  # Counter
        offset = 2  # Offset
        while counter < 4096:
            V = []
            for primeIndex in range(primeCount + 1):
                arg = xmpz((seed + offset + primeIndex) % (2 ** primeBitLength))
                zzv = sha1(to_binary(arg)).hexdigest()
                V.append(int(zzv, 16))
            W_acc = 0
            for primeIndex in range(0, primeCount):
                W_acc += V[primeIndex] * 2 ** (160 * primeIndex)
            W_acc += (V[primeCount] % 2 ** bitCount) * 2 ** (160 * primeCount)
            X = W_acc + 2 ** (bitLength - 1)
            c = X % (2 * q)
            p = X - c + 1  # p = X - (c - 1)
            if p >= 2 ** (bitLength - 1):
                if is_prime(p, 10):
                    return p, q
            counter += 1
            offset += primeCount + 1


def generateG(p, q):
    # Generate a generator g
    while True:
        h = randrange(2, p - 1)
        exponent = xmpz((p - 1) // q)
        g = powmod(h, exponent, p)
        if g > 1:
            break
    return g


def generateKeys(g, p, q):
    # Generate public and private keys
    privateKey = randrange(2, q)  # privateKey < q
    publicKey = powmod(g, privateKey, p)
    return privateKey, publicKey


def generateParameters(bitLength, securityParameter):
    # Generate prime number parameters p, q, and generator g
    p, q = generatePQ(bitLength, securityParameter)
    g = generateG(p, q)
    return p, q, g


def sign(message, p, q, g, privateKey):
    # Generate a signature for the message
    if not validateParameters(p, q, g):
        raise Exception("Invalid parameters")
    while True:
        k = randrange(2, q)  # k < q
        r = powmod(g, k, p) % q
        messageHash = int(sha1(message).hexdigest(), 16)
        try:
            s = (invert(k, q) * (messageHash + privateKey * r)) % q
            return r, s
        except ZeroDivisionError:
            pass


def verify(message, r, s, p, q, g, publicKey):
    # Verify the signature of a message
    if not validateParameters(p, q, g):
        raise Exception("Invalid parameters")
    if not validateSignature(r, s, q):
        return False
    try:
        w = invert(s, q)
    except ZeroDivisionError:
        return False
    messageHash = int(sha1(message).hexdigest(), 16)
    u1 = (messageHash * w) % q
    u2 = (r * w) % q
    v = (powmod(g, u1, p) * powmod(publicKey, u2, p)) % p % q
    if v == r:
        return True
    return False


def validateParameters(p, q, g):
    # Validate the parameters p, q, g
    if is_prime(p) and is_prime(q):
        return True
    if powmod(g, q, p) == 1 and g > 1 and (p - 1) % q:
        return True
    return False


def validateSignature(r, s, q):
    # Validate the signature values r, s
    if r < 0 or r > q:
        return False
    if s < 0 or s > q:
        return False
    return True


securityParameter = 160
bitLength = 1024
p, q, g = generateParameters(bitLength, securityParameter)
privateKey, publicKey = generateKeys(g, p, q)

message = "Some message"
messageBytes = str.encode(message, "ascii")
r, s = sign(messageBytes, p, q, g, privateKey)
print("public key: ", publicKey)
print("private key: ", privateKey)
if verify(messageBytes, r, s, p, q, g, publicKey):
    print('Correct')
