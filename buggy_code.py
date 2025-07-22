# Bug Example 1: Syntax Errors and Logic Issues
def greeting(name)  # Bug: Missing colon
    print("Hello" + name)  # Bug: Missing space in concatenation
    
greeting("Adeeb  # Bug: Unclosed string literal

# Bug Example 2: Security Vulnerability - SQL Injection
import sqlite3

def get_user_data(user_id):
    conn = sqlite3.connect('users.db')
    cursor = conn.cursor()
    # Bug: SQL injection vulnerability
    query = f"SELECT * FROM users WHERE id = {user_id}"
    cursor.execute(query)
    result = cursor.fetchall()
    conn.close()
    return result

# Bug Example 3: Performance Issue - Inefficient Algorithm
def find_duplicates(arr):
    duplicates = []
    # Bug: O(n²) time complexity - inefficient nested loops
    for i in range(len(arr)):
        for j in range(i + 1, len(arr)):
            if arr[i] == arr[j] and arr[i] not in duplicates:
                duplicates.append(arr[i])
    return duplicates

# Bug Example 4: Logic Error - Off-by-one error
def get_last_n_items(items, n):
    # Bug: This will miss the last item when n equals list length
    return items[len(items) - n:len(items) - 1]

# Bug Example 5: Resource Management Issue
def process_file(filename):
    # Bug: File not properly closed, potential resource leak
    file = open(filename, 'r')
    data = file.read()
    return data.upper()