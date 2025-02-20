outdir=/tmp/DVA246_pq

test:
	@mkdir -m 700 -p $(outdir)
	mcs *.cs -out:$(outdir)/priorityqueue && $(outdir)/priorityqueue

clean:
	@rm -rf $(outdir)/*

edit:
	code -n *.cs makefile &

install:
	sudo apt-get install mono-devel libatk-adaptor libgail-common
