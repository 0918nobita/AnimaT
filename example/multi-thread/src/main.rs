use std::sync::{Arc, Mutex};
use std::thread;
use std::time::Duration;

fn main() {
    let shared_data = Arc::new(Mutex::new(vec![1, 2, 3]));

    for i in 0..3 {
        let data_ref = shared_data.clone();
        thread::spawn(move || {
            let mut mutex_data = data_ref.lock().unwrap();
            mutex_data[i] += 1;
        });
    }

    thread::sleep(Duration::from_secs(3));

    println!("{:?}", shared_data); // => Mutex { data: [2, 3, 4] }
}
