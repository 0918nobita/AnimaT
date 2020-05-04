extern crate vulkano;
extern crate vulkano_win;
extern crate winit;

use std::sync::mpsc;
use std::thread;
use std::time::Duration;

use vulkano::instance::{ApplicationInfo, Instance, InstanceExtensions, Version};

use winit::{
    dpi::LogicalSize,
    event::{Event, WindowEvent},
    event_loop::{ControlFlow, EventLoop},
    window::WindowBuilder,
};

const WIDTH: u32 = 800;
const HEIGHT: u32 = 600;

fn main() {
    let (sender, receiver) = mpsc::channel();

    let sub_sender = mpsc::Sender::clone(&sender);
    thread::spawn(move || {
        let vals = vec![
            String::from("hi"),
            String::from("from"),
            String::from("the"),
            String::from("thread"),
        ];

        for val in vals {
            sub_sender.send(val).unwrap();
            thread::sleep(Duration::from_secs(1));
        }
    });

    thread::spawn(move || {
        let vals = vec![
            String::from("more"),
            String::from("messages"),
            String::from("for"),
            String::from("you"),
        ];

        for val in vals {
            sender.send(val).unwrap();
            thread::sleep(Duration::from_secs(1));
        }
    });

    for received in receiver {
        println!("Got: {}", received);
    }

    let supported_exts = InstanceExtensions::supported_by_core().unwrap();
    println!("Supported extensions {:?}", supported_exts);

    let app_info = ApplicationInfo {
        application_name: Some("AnimaT".into()),
        application_version: Some(Version {
            major: 1,
            minor: 0,
            patch: 0,
        }),
        engine_name: Some("No engine".into()),
        engine_version: Some(Version {
            major: 1,
            minor: 0,
            patch: 0,
        }),
    };

    let required_exts = vulkano_win::required_extensions();

    Instance::new(Some(&app_info), &required_exts, None).expect("failed to create Vulkan instance");

    let event_loop = EventLoop::new();

    let window = WindowBuilder::new().build(&event_loop).unwrap();

    window.set_title("Vulkan");
    window.set_inner_size(LogicalSize::new(f64::from(WIDTH), f64::from(HEIGHT)));
    window.set_resizable(false);
    window.set_visible(true);

    event_loop.run(move |event, _, control_flow| {
        *control_flow = ControlFlow::Wait;

        match event {
            Event::WindowEvent {
                event: WindowEvent::CloseRequested,
                window_id,
            } if window_id == window.id() => *control_flow = ControlFlow::Exit,
            _ => (),
        }
    });
}
