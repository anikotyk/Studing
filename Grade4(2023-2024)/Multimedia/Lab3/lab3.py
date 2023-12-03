import cv2
import numpy as np
from math import log10, sqrt

#global values
 
eps = 1e-10

block_size = 8

#dwt 

def dwt_h1(size: int) -> np.ndarray:
    h = np.zeros((size, size))
    for i in range(0, size, 2):
        h[i, i // 2] = 0.5
        h[i + 1, i // 2] = 0.5
    for i in range(size-1, -1, -2):
        h[i,  size - (size - i + 1) // 2] = -0.5
        h[i - 1,  size - (size - i + 1) // 2] = 0.5
    return h


def dwt_h2(size: int) -> np.ndarray:
    h = np.zeros((size, size))
    h[:size // 2, :size // 2] = dwt_h1(size // 2)
    h[size // 2:, size // 2:] = np.eye(size // 2)
    return h


def dwt_h3(size: int) -> np.ndarray:
    h = np.eye(size)
    h[0, 0] = h[0, 1] = h[1, 0] = 0.5
    h[1, 1] = -0.5
    return h


def dwt_h(size: int) -> np.ndarray:
    h1, h2, h3 = dwt_h1(size), dwt_h2(size), dwt_h3(size)
    return np.matmul(np.matmul(h1, h2), h3)


def dwt_compress(image: np.ndarray, compression_ratio: float = 1.5) -> np.ndarray:
    size = image.shape[0]
    h = dwt_h(size)
    h_t = np.transpose(h)
    b = np.matmul(np.matmul(h_t, image), h)

    non_zero = round(np.sum(np.abs(b) > eps) / compression_ratio)
    to_zero = b.size - non_zero

    b_flat = b.flatten()
    sorted_indices = np.argsort(b_flat)
    b_flat[sorted_indices[:to_zero]] = 0
    b = b_flat.reshape(b.shape)
    return b


def dwt_decompress(encoded_image: np.ndarray) -> np.ndarray:
    size = encoded_image.shape[0]
    h = dwt_h(size)
    h_t = np.transpose(h)
    decompressed = np.matmul(np.matmul(np.linalg.inv(h_t), encoded_image), np.linalg.inv(h))
    decompressed = np.clip(np.round(decompressed), 0, 255)
    return decompressed.astype(np.uint8)

def psnr(original, compressed):
    mse = np.mean((original - compressed) ** 2)
    if mse == 0:
        return 100
    max_pixel = 255.0
    psnr_value = 20 * log10(max_pixel / sqrt(mse))
    return psnr_value

#dct


def quality_matrix(quality: int) -> np.ndarray:
    q50 = np.array([[16, 11, 10, 16, 24, 40, 51, 61],
                     [12, 12, 14, 19, 26, 58, 60, 55],
                     [14, 13, 16, 24, 40, 57, 69, 56],
                     [14, 17, 22, 29, 51, 87, 80, 62],
                     [18, 22, 37, 56, 68, 109, 103, 77],
                     [24, 35, 55, 64, 81, 104, 113, 92],
                     [49, 64, 78, 87, 103, 121, 120, 101],
                     [72, 92, 95, 98, 112, 100, 103, 99]])
    
    q = q50 if quality == 50 else (q50 * (100 - quality) / 50 if quality > 50 else q50 * 50 / quality)
    q = np.clip(np.round(q), 1, 255).astype(int)

    return q


def to_zigzag(matrix: np.ndarray) -> list:
    zigzag_list = []
    for i in range(2 * block_size - 1):
        if i % 2 == 0:
            for j in range(max(0, i - block_size + 1), min(i, block_size - 1) + 1):
                zigzag_list.append(matrix[i - j, j])
        else:
            for j in range(max(0, i - block_size + 1), min(i, block_size - 1) + 1):
                zigzag_list.append(matrix[j, i - j])
    return zigzag_list


def from_zigzag(zigzag_list: list) -> np.ndarray:
    matrix = np.zeros((block_size, block_size))
    count = 0
    for i in range(2 * block_size - 1):
        if i % 2 == 0:
            for j in range(max(0, i - block_size + 1), min(i, block_size - 1) + 1):
                matrix[i - j, j] = zigzag_list[count]
                count += 1
        else:
            for j in range(max(0, i - block_size + 1), min(i, block_size - 1) + 1):
                matrix[j, i - j] = zigzag_list[count]
                count += 1
    return matrix


def run_length_encoding(data: list) -> list:
    encoded_data = []
    current_num = data[0]
    count = 1

    for num in data[1:]:
        if num == current_num:
            count += 1
        else:
            if count == 1:
                encoded_data.append((current_num, ))
            else:
                encoded_data.append((current_num, count))
            current_num = num
            count = 1

    encoded_data.append((current_num, count))
    return encoded_data


def run_length_decoding(encoded_data):
    data = []
    for elem in encoded_data:
        if len(elem) == 1:
            data.append(elem[0])
        else:
            num, count = elem
            data.extend([num] * count)
    return data


def dct_compress(image: np.ndarray, quality: int = 50) -> list:
    q = quality_matrix(quality)

    a = 1 / np.sqrt(block_size)
    b = np.sqrt(2 / block_size)
    t = np.array(
        [[a if i == 0 else b * np.cos((2*j+1) * i * np.pi / (2*block_size)) for j in range(block_size)]
         for i in range(block_size)])  

    encoded_image = [quality]
    for block_i in range(0, image.shape[0], block_size):
        encoded_row = []
        for block_j in range(0, image.shape[1], block_size):
            block = image[block_i:block_i + block_size, block_j:block_j + block_size].astype(int)
            m = block - 128
            d = np.matmul(np.matmul(t, m), np.transpose(t))
            c = np.round(d / q).astype(int)
            encoded_block = run_length_encoding(to_zigzag(c))

            encoded_row.append(encoded_block)
        encoded_image.append(encoded_row)

    return encoded_image


def dct_decompress(encoded_image: list) -> np.ndarray:
    quality, *encoded_rows = encoded_image
    q = quality_matrix(quality)
    image = np.zeros((len(encoded_rows) * block_size, ) * 2, dtype=np.uint8)

    a = 1 / np.sqrt(block_size)
    b = np.sqrt(2 / block_size)
    t = np.array(
        [[a if i == 0 else b * np.cos((2*j+1) * i * np.pi / (2*block_size)) for j in range(block_size)]
         for i in range(block_size)])   

    for encoded_row, block_i in zip(encoded_rows, range(0, image.shape[0], block_size)):
        for encoded_block, block_j in zip(encoded_row, range(0, image.shape[1], block_size)):
            c = from_zigzag(run_length_decoding(encoded_block))
            r = q * c
            n = np.clip(np.round(np.matmul(np.matmul(np.transpose(t), r), t)) + 128, 0, 255)
            image[block_i:block_i + block_size, block_j:block_j + block_size] = n
    return image

def main():
    input_image = cv2.imread("image.jpg", 0)
    decompressed_image_dct: np.ndarray
    decompressed_image_dwt: np.ndarray
   
    dct_quality = 50
    compressed_image_dct = dct_compress(input_image, dct_quality)
    decompressed_image_dct = dct_decompress(compressed_image_dct)
    cv2.imwrite("dct_decompressed_image.jpg", decompressed_image_dct)
    print(f"Dct psnr: {psnr(input_image, decompressed_image_dct)}")
    
    dwt_compression_ratio = 0.1
    compressed_image_dwt = dwt_compress(input_image, dwt_compression_ratio)
    decompressed_image_dwt = dwt_decompress(compressed_image_dwt)
    cv2.imwrite("dwt_decompressed_image.jpg", decompressed_image_dwt)
    print(f"Dwt psnr: {psnr(input_image, decompressed_image_dwt)}")


if __name__ == '__main__':
    main()
