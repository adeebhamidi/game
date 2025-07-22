# Fix 1: Syntax Errors and Logic Issues - FIXED
def greeting(name):  # Fixed: Added missing colon
    print("Hello " + name)  # Fixed: Added space in concatenation for proper formatting
    
greeting("Adeeb")  # Fixed: Properly closed string literal

# Fix 2: Security Vulnerability - SQL Injection - FIXED
import sqlite3

def get_user_data(user_id):
    conn = sqlite3.connect('users.db')
    cursor = conn.cursor()
    # Fixed: Using parameterized query to prevent SQL injection
    query = "SELECT * FROM users WHERE id = ?"
    cursor.execute(query, (user_id,))
    result = cursor.fetchall()
    conn.close()
    return result

# Fix 3: Performance Issue - Inefficient Algorithm - FIXED
def find_duplicates(arr):
    # Fixed: Using set for O(n) time complexity instead of O(n²)
    seen = set()
    duplicates = set()
    
    for item in arr:
        if item in seen:
            duplicates.add(item)
        else:
            seen.add(item)
    
    return list(duplicates)

# Fix 4: Logic Error - Off-by-one error - FIXED
def get_last_n_items(items, n):
    # Fixed: Proper slicing to get last n items
    if n <= 0:
        return []
    return items[-n:]  # Much simpler and correct

# Fix 5: Resource Management Issue - FIXED
def process_file(filename):
    # Fixed: Using context manager to ensure file is properly closed
    try:
        with open(filename, 'r') as file:
            data = file.read()
            return data.upper()
    except FileNotFoundError:
        return "File not found"
    except Exception as e:
        return f"Error processing file: {str(e)}"

# Additional improvement: Input validation for greeting function
def safe_greeting(name):
    """Enhanced greeting function with input validation"""
    if not isinstance(name, str):
        raise TypeError("Name must be a string")
    if not name.strip():
        raise ValueError("Name cannot be empty")
    
    # Sanitize input to prevent potential issues
    clean_name = name.strip()
    return f"Hello {clean_name}!"

# Example usage with error handling
if __name__ == "__main__":
    # Test the fixed functions
    print("Testing fixed functions:")
    
    # Test greeting
    greeting("Adeeb")
    
    # Test safe greeting with validation
    try:
        result = safe_greeting("Adeeb")
        print(result)
    except (TypeError, ValueError) as e:
        print(f"Error: {e}")
    
    # Test find_duplicates performance improvement
    test_array = [1, 2, 3, 2, 4, 5, 3, 6, 1]
    duplicates = find_duplicates(test_array)
    print(f"Duplicates found: {duplicates}")
    
    # Test get_last_n_items
    items = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
    last_3 = get_last_n_items(items, 3)
    print(f"Last 3 items: {last_3}")