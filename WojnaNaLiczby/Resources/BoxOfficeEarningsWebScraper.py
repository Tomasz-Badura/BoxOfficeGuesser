import requests
import csv
import time
from bs4 import BeautifulSoup

# Web crawler that gets Worldwide Box Office earnings from:
# https://www.the-numbers.com/box-office-records/worldwide/all-movies/cumulative/all-time
# Used to generate the "BoxOfficeMovieEarnings.csv" file
# The file was further modified to remove broken or unnecessary entries    

def fetch_page(url, headers):
    try:
        response = requests.get(url, headers=headers)
        response.raise_for_status()
        return response
    except requests.RequestException as e:
        print(e)
        return None

def parse_table_data(soup):
    table_data = []
    table = soup.find('table')
    if table is None: # end of data
        return table_data

    for row in table.find_all('tr')[1:]:
        cols = row.find_all('td')
        if len(cols) == 6:
            year = cols[1].get_text(strip=True).split('>')[-1].split('<')[0] 
            movie = cols[2].get_text(strip=True)
            worldwide = cols[3].get_text(strip=True).replace('$', '').replace(',', '')

            try:
                worldwide = int(worldwide)
            except:
                worldwide = None
            
            table_data.append({
                'Year': year,
                'Movie': movie,
                'Income': worldwide,
            })
    
    return table_data

def save_to_csv(data, filename):
    with open(filename, 'w', newline='', encoding='utf-8') as file:
        writer = csv.writer(file)
        writer.writerow(['Year', 'Movie', 'Income'])
        for row in data:
            writer.writerow([
                row['Year'],
                row['Movie'],
                row['Income'] if row['Income'] is not None else 'N/A',
            ])

def main():
    base_url = 'https://www.the-numbers.com/box-office-records/worldwide/all-movies/cumulative/all-time'
    current_url = base_url
    sub = 101
    headers = {
        'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36'
    }
    
    all_data = []
    while True:
        response = fetch_page(current_url, headers)
        if response is None:
            break
        
        soup = BeautifulSoup(response.content, 'html.parser')
        page_data = parse_table_data(soup)
        if not page_data:
            break
        
        all_data.extend(page_data)
        
        current_url = base_url + '/' + str(sub)
        sub += 100
        time.sleep(1) # rate limit 

    save_to_csv(all_data, "BoxOfficeMovieEarnings.csv")

if __name__ == "__main__":
    main()