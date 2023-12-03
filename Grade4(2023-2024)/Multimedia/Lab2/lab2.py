import os
os.add_dll_directory(os.getcwd())
import vlc
import tkinter as tk
from tkinter import filedialog
from datetime import timedelta
from PIL import Image, ImageTk

class MediaPlayer(tk.Tk):
    def __init__(self):
        super().__init__()
        self.title("Mедіаплеєр")
        self.initializePlayer()

    def initializePlayer(self):
        self.instance = vlc.Instance()
        self.mediaPlayer = vlc.MediaPlayer()
        self.currentFilePath = None
        self.isPlayingVideo = False
        self.isVideoPaused = False
        self.setupInterface()
        self.mediaPlayer.event_manager().event_attach(vlc.EventType.MediaPlayerEndReached, self.handleEndReached)
        
    def handleEndReached(self, event):
        self.after(1, self.handleEndReachedOnMainThread)

    def handleEndReachedOnMainThread(self):
        self.stop()

    def setupInterface(self):
        self.canvas = tk.Canvas(self, bg="black", width=800, height=400)
        self.canvas.pack(pady=0, fill=tk.BOTH, expand=True)
        self.menuBar = tk.Menu(self)
        self.fileMenu = tk.Menu(self.menuBar, tearoff=0)
        self.fileMenu.add_command(label="Open", command=self.selectFile)
        self.menuBar.add_cascade(label="File", menu=self.fileMenu)
        self.config(menu=self.menuBar)

        self.progressBar = CustomProgressBar(self, self.setVideoPosition, bg="#e0e0e0", highlightthickness=0)
        self.progressBar.pack(fill=tk.X, padx=5, pady=0)

        self.timeLabel = tk.Label(
            self,
            text="00:00:00 / 00:00:00",
            font=("Courier", 12, "bold"),
            fg="#555555",
            bg="#f0f0f0",
        )
        self.timeLabel.pack(pady=5)

        self.buttonsFrame = tk.Frame(self, bg="#f0f0f0")
        self.buttonsFrame.pack(pady=5)

        playImage = Image.open("play.png")
        playImageResized = playImage.resize((20, 20), Image.Resampling.LANCZOS)
        self.iconPlay = ImageTk.PhotoImage(playImageResized)

        pauseImage = Image.open("pause.png")
        pauseImageResized = pauseImage.resize((20, 20), Image.Resampling.LANCZOS)
        self.iconPause = ImageTk.PhotoImage(pauseImageResized)

        stopImage = Image.open("stop.png")
        stopImageResized = stopImage.resize((25, 25), Image.Resampling.LANCZOS)
        self.iconStop = ImageTk.PhotoImage(stopImageResized)

        rewindImage = Image.open("rewind.png")
        rewindImageResized = rewindImage.resize((20, 20), Image.Resampling.LANCZOS)
        self.iconRewind = ImageTk.PhotoImage(rewindImageResized)

        fastForwardImage = Image.open("fastforward.png")
        fastForwardImageResized = fastForwardImage.resize((20, 20), Image.Resampling.LANCZOS)
        self.iconFastForward = ImageTk.PhotoImage(fastForwardImageResized)

        self.playPauseButton = tk.Button(
            self.buttonsFrame,
            width=25,
            height=25,
            image=self.iconPlay,
            command=self.togglePlayPause,
        )
        self.playPauseButton.pack(side=tk.LEFT, padx=5, pady=5)

        self.stopButton = tk.Button(
            self.buttonsFrame,
            width=25,
            height=25,
            image=self.iconStop,
            command=self.stop,
        )
        self.stopButton.pack(side=tk.LEFT, pady=5)

        self.rewindButton = tk.Button(
            self.buttonsFrame,
            width=25,
            height=25,
            image=self.iconRewind,
            command=self.rewind,
        )
        self.rewindButton.pack(side=tk.LEFT, padx=5, pady=5)

        self.fastForwardButton = tk.Button(
            self.buttonsFrame,
            width=25,
            height=25,
            image=self.iconFastForward,
            command=self.fastForward,
        )
        self.fastForwardButton.pack(side=tk.LEFT, padx=5, pady=5)

    def selectFile(self):
        file_path = filedialog.askopenfilename(
            filetypes=[("Media Files", "*.mp4 *.webm *.mkv *.mp3 *.avi *.mov *.wav *.ogg")]
        )
        if file_path:
            self.stop()
            self.currentFilePath = file_path
            self.timeLabel.config(text="00:00:00 / " + self.getDurationStr())
            self.canvas.delete("all")
            media = self.instance.media_new(self.currentFilePath)
            self.mediaPlayer.set_media(media)
            self.mediaPlayer.set_hwnd(self.canvas.winfo_id())
            self.play()

    def getDurationStr(self):
        if self.isPlayingVideo and self.currentFilePath:
            total_duration = self.mediaPlayer.get_length()
            total_duration_str = str(timedelta(milliseconds=total_duration)).split(".")[0]
            return total_duration_str
        return "00:00:00"

    def rewind(self):
        if self.isPlayingVideo and self.currentFilePath:
            current_time = self.mediaPlayer.get_time() - 10000
            if current_time < 0:
                current_time = 0
            self.mediaPlayer.set_time(current_time)

    def fastForward(self):
        if self.isPlayingVideo and self.currentFilePath:
            current_time = self.mediaPlayer.get_time() + 10000
            if current_time > self.mediaPlayer.get_length():
                current_time = self.mediaPlayer.get_length()
            self.mediaPlayer.set_time(current_time)

    def togglePlayPause(self):
        if self.currentFilePath:
            if not self.isPlayingVideo or self.isVideoPaused:
                self.play()
            else:
                self.pause()

    def play(self):
        if self.currentFilePath:
            self.mediaPlayer.play()
            self.isVideoPaused = False
            self.isPlayingVideo = True
            self.playPauseButton.config(image=self.iconPause)

    def pause(self):
        if self.isPlayingVideo and self.currentFilePath:
            self.mediaPlayer.pause()
            self.isVideoPaused = True
            self.playPauseButton.config(image=self.iconPlay)

    def stop(self):
        if self.isPlayingVideo and self.currentFilePath:
            self.mediaPlayer.stop()
            self.isPlayingVideo = False
        self.timeLabel.config(text="00:00:00 / " + self.getDurationStr())
        self.playPauseButton.config(image=self.iconPlay)
        self.progressBar.set(0)

    def setVideoPosition(self, value):
        if not self.progressBar.isUserDragging:
            return
        if self.isPlayingVideo and self.currentFilePath:
            total_duration = self.mediaPlayer.get_length()
            position = int((float(value) / 100) * total_duration)
            self.mediaPlayer.set_time(position)

    def updateVideoProgress(self):
        if self.isPlayingVideo and self.currentFilePath:
            total_duration = self.mediaPlayer.get_length()
            if total_duration > 0:
                current_time = self.mediaPlayer.get_time()
                progress_percentage = (current_time / total_duration) * 100
                self.progressBar.set(progress_percentage)
                current_time_str = str(timedelta(milliseconds=current_time)).split(".")[0]
                total_duration_str = str(timedelta(milliseconds=total_duration)).split(".")[0]
                self.timeLabel.config(text=f"{current_time_str}/{total_duration_str}")
        self.after(100, self.updateVideoProgress)

class CustomProgressBar(tk.Scale):
    def __init__(self, master, command, **kwargs):
        kwargs["showvalue"] = False
        super().__init__(
            master,
            from_=0,
            to=100,
            orient=tk.HORIZONTAL,
            length=800,
            command=command,
            **kwargs,
        )

        self.bind("<Button-1>", self.onClick)
        self.isUserDragging = False 
        self.bind("<ButtonPress-1>", self.onDragStart)
        self.bind("<ButtonRelease-1>", self.onDragStop)

    def onDragStart(self, event):
        self.isUserDragging = True

    def onDragStop(self, event):
        self.isUserDragging = False

    def onClick(self, event):
        if self.cget("state") == tk.NORMAL:
            value = (event.x / self.winfo_width()) * 100
            self.set(value)

player = MediaPlayer()
player.updateVideoProgress()
player.mainloop()
